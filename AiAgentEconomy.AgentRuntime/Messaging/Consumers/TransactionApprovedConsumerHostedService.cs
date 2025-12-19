using AiAgentEconomy.AgentRuntime.Memory;
using AiAgentEconomy.AgentRuntime.Messaging.Abstractions;
using AiAgentEconomy.AgentRuntime.Messaging.Contracts;
using AiAgentEconomy.AgentRuntime.Observability;
using AiAgentEconomy.AgentRuntime.Orchestration.Ports;
using AiAgentEconomy.AgentRuntime.Orchestration.Retry;
using AiAgentEconomy.AgentRuntime.Policies;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AiAgentEconomy.AgentRuntime.Messaging.Consumers;

public sealed class TransactionApprovedConsumerHostedService(
    IMessageBus bus,
    IPolicyEvaluator policy,
    IAuditWriter audit,
    IProcessedEventStore processedEvents,
    ITransactionLifecycleClient lifecycle,
    ILogger<TransactionApprovedConsumerHostedService> logger
) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        bus.Subscribe<TransactionApproved>(HandleAsync);
        logger.LogInformation("TransactionApproved consumer subscribed.");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("TransactionApproved consumer stopped.");
        return Task.CompletedTask;
    }

    private async Task HandleAsync(TransactionApproved evt, CancellationToken ct)
    {
        logger.LogInformation(
            "Received TransactionApproved txId={TxId} agentId={AgentId} amount={Amount} {Currency}",
            evt.TransactionId, evt.AgentId, evt.Amount, evt.Currency);

        // 1) Policy check
        var decision = await policy.EvaluateAsync(
            new PolicyContext(evt.AgentId, evt.TransactionId, evt.Amount, evt.Currency),
            ct);

        await audit.WriteAsync(
            new AuditRecord(
                EventType: "TransactionApproved.Consumed",
                CorrelationId: evt.CorrelationId,
                Actor: $"agent:{evt.AgentId}",
                OccurredAt: DateTimeOffset.UtcNow,
                Data: new { evt.TransactionId, evt.Amount, evt.Currency, decision.Allowed, decision.Reason }
            ),
            ct
        );

        if (!decision.Allowed)
        {
            logger.LogWarning("Policy denied txId={TxId}. Reason={Reason}", evt.TransactionId, decision.Reason);
            return;
        }

        // 2) Idempotency (lease-based)
        var eventKey = $"TransactionApproved:{evt.TransactionId}";
        var begin = await processedEvents.TryBeginAsync(eventKey, lease: TimeSpan.FromMinutes(2), ct);

        if (begin == BeginResult.AlreadyCompleted)
        {
            logger.LogInformation("Duplicate event ignored (already completed). eventKey={EventKey}", eventKey);
            return;
        }

        if (begin == BeginResult.AlreadyInProgress)
        {
            logger.LogWarning("Duplicate event ignored (in progress). eventKey={EventKey}", eventKey);
            return;
        }

        // 3) Submit pipeline (with retry)
        try
        {
            await audit.WriteAsync(
                new AuditRecord(
                    EventType: "Transaction.SubmitRequested",
                    CorrelationId: evt.CorrelationId,
                    Actor: $"agent:{evt.AgentId}",
                    OccurredAt: DateTimeOffset.UtcNow,
                    Data: new { evt.TransactionId }
                ),
                ct
            );

            // API'nin beklediği body
            var submitRequest = new SubmitTransactionHttpRequest(
                                                                    Chain: "Arbitrum",
                                                                    Network: "arbitrum-sepolia",
                                                                    ExplorerUrl: null
                                                                );

            await RetryPolicy.ExecuteAsync(
                action: innerCt => lifecycle.SubmitAsync(evt.TransactionId, submitRequest, innerCt),
                maxAttempts: 3,
                initialDelay: TimeSpan.FromSeconds(1),
                logger: logger,
                operationName: $"Submit txId={evt.TransactionId}",
                ct: ct
            );

            await audit.WriteAsync(
                new AuditRecord(
                    EventType: "Transaction.SubmitSucceeded",
                    CorrelationId: evt.CorrelationId,
                    Actor: $"agent:{evt.AgentId}",
                    OccurredAt: DateTimeOffset.UtcNow,
                    Data: new { evt.TransactionId }
                ),
                ct
            );

            // MVP: submit sonrası settle
            await audit.WriteAsync(
                new AuditRecord(
                    EventType: "Transaction.SettleRequested",
                    CorrelationId: evt.CorrelationId,
                    Actor: $"agent:{evt.AgentId}",
                    OccurredAt: DateTimeOffset.UtcNow,
                    Data: new { evt.TransactionId }
                ),
                ct
            );

            await RetryPolicy.ExecuteAsync(
                action: innerCt => lifecycle.SettleAsync(evt.TransactionId, innerCt),
                maxAttempts: 3,
                initialDelay: TimeSpan.FromSeconds(1),
                logger: logger,
                operationName: $"Settle txId={evt.TransactionId}",
                ct: ct
            );

            await audit.WriteAsync(
                new AuditRecord(
                    EventType: "Transaction.SettleSucceeded",
                    CorrelationId: evt.CorrelationId,
                    Actor: $"agent:{evt.AgentId}",
                    OccurredAt: DateTimeOffset.UtcNow,
                    Data: new { evt.TransactionId }
                ),
                ct
            );

            // Completed işaretini en sona koy
            await processedEvents.MarkCompletedAsync(eventKey, ct);

            logger.LogInformation("Submit+Settle pipeline completed for txId={TxId}", evt.TransactionId);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Submit pipeline failed for txId={TxId}", evt.TransactionId);

            await audit.WriteAsync(
                new AuditRecord(
                    EventType: "Transaction.SubmitFailed",
                    CorrelationId: evt.CorrelationId,
                    Actor: $"agent:{evt.AgentId}",
                    OccurredAt: DateTimeOffset.UtcNow,
                    Data: new { evt.TransactionId, Error = ex.Message }
                ),
                ct
            );

            // Not completed -> lease dolarsa yeniden denenebilir (MVP).
        }
    }
}
