using AiAgentEconomy.AgentRuntime.Messaging.Abstractions;
using AiAgentEconomy.AgentRuntime.Messaging.Contracts;

namespace AiAgentEconomy.AgentRuntime
{
    public sealed class Worker(
        ILogger<Worker> logger,
        IConfiguration config,
        IMessageBus bus
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("AgentRuntime started.");

            // Smoke publish (default false)
            var enableSmoke = config.GetValue<bool>("AgentRuntime:EnableSmokePublish");
            if (enableSmoke)
            {
                // Burayý ister sabit bir txId ile test için kullanýrsýn,
                // default kapalý kalsýn.
                var evt = new TransactionApproved(
                    TransactionId: Guid.Parse("50e6ae7d-49dc-4556-a541-9a91dd09cd4b"), // testte deðiþtir
                    AgentId: Guid.NewGuid(),
                    Amount: 5m,
                    Currency: "USDC",
                    CorrelationId: Guid.NewGuid().ToString("N"),
                    OccurredAt: DateTimeOffset.UtcNow
                );

                await bus.PublishAsync(evt, stoppingToken);
                logger.LogInformation("Smoke publish executed. txId={TxId}", evt.TransactionId);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("AgentRuntime heartbeat at {Time}.", DateTimeOffset.UtcNow);
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
