using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.AgentRuntime.Memory
{
    public enum BeginResult
    {
        Begun,
        AlreadyInProgress,
        AlreadyCompleted
    }

    public interface IProcessedEventStore
    {
        Task<BeginResult> TryBeginAsync(string eventKey, TimeSpan lease, CancellationToken ct = default);
        Task MarkCompletedAsync(string eventKey, CancellationToken ct = default);
    }
}
