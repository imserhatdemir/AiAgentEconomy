using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.AgentRuntime.Orchestration.Ports
{
    public interface ITransactionLifecycleClient
    {
        Task SubmitAsync(Guid transactionId, SubmitTransactionHttpRequest request, CancellationToken ct = default);
        Task SettleAsync(Guid transactionId, CancellationToken ct = default);
        Task FailAsync(Guid transactionId, string reason, CancellationToken ct = default);
    }
}
