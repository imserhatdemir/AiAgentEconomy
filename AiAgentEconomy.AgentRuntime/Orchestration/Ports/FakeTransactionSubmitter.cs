using System;
using System.Collections.Concurrent;

namespace AiAgentEconomy.AgentRuntime.Orchestration.Ports
{
    public sealed class FakeTransactionSubmitter(ILogger<FakeTransactionSubmitter> logger) : ITransactionSubmitter
    {
        private static readonly ConcurrentDictionary<Guid, int> _attempts = new();

        public Task SubmitAsync(Guid transactionId, CancellationToken ct = default)
        {
            var attempt = _attempts.AddOrUpdate(transactionId, 1, (_, v) => v + 1);

            logger.LogInformation("FAKE-SUBMIT: Attempt {Attempt} for txId={TxId}", attempt, transactionId);

            // İlk 2 denemede hata, 3. denemede başarı
            if (attempt < 3)
                throw new Exception("Simulated transient submit failure.");

            logger.LogInformation("FAKE-SUBMIT: Success for txId={TxId}", transactionId);
            return Task.CompletedTask;
        }
    }
}
