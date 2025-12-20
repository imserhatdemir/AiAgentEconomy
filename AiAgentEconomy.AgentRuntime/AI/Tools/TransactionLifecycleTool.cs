using AiAgentEconomy.AgentRuntime.Orchestration.Ports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.AgentRuntime.AI.Tools
{
    public sealed class TransactionLifecycleTool(ITransactionLifecycleClient lifecycle)
    {
        [Description("Submits a transaction on-chain via API lifecycle endpoint.")]
        public async Task<string> SubmitAsync(Guid transactionId, CancellationToken ct)
        {
            await lifecycle.SubmitAsync(transactionId, ct);
            return $"Submitted transaction {transactionId}.";
        }

        [Description("Settles a previously submitted transaction via API lifecycle endpoint.")]
        public async Task<string> SettleAsync(Guid transactionId, CancellationToken ct)
        {
            await lifecycle.SettleAsync(transactionId, ct);
            return $"Settled transaction {transactionId}.";
        }

        [Description("Fails a transaction via API lifecycle endpoint with a reason code.")]
        public async Task<string> FailAsync(Guid transactionId, string reason, CancellationToken ct)
        {
            await lifecycle.FailAsync(transactionId, reason, ct);
            return $"Failed transaction {transactionId}. Reason={reason}";
        }
    }
}
