using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.AgentRuntime.Messaging.Abstractions
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class;
        void Subscribe<T>(Func<T, CancellationToken, Task> handler) where T : class;
    }
}
