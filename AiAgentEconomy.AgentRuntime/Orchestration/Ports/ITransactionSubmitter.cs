using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.AgentRuntime.Orchestration.Ports
{
    public interface ITransactionSubmitter
    {
        Task SubmitAsync(Guid transactionId, CancellationToken ct = default);
    }
}
