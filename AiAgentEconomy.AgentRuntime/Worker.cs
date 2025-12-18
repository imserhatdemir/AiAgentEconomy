using AiAgentEconomy.AgentRuntime.Messaging.Abstractions;
using AiAgentEconomy.AgentRuntime.Messaging.Contracts;

namespace AiAgentEconomy.AgentRuntime
{
    public sealed class Worker(ILogger<Worker> logger, IMessageBus bus) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("AgentRuntime started.");

            // One-time smoke event
            var evt = new TransactionApproved(
                            TransactionId: Guid.NewGuid(),
                            AgentId: Guid.NewGuid(),
                            Amount: 25.50m,
                            Currency: "USDC",
                            CorrelationId: Guid.NewGuid().ToString("N"),
                            OccurredAt: DateTimeOffset.UtcNow
                        );
            await bus.PublishAsync(evt, stoppingToken);
            await bus.PublishAsync(evt, stoppingToken); // duplicate test

            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("AgentRuntime heartbeat at {Time}.", DateTimeOffset.UtcNow);
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
