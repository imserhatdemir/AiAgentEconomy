using AiAgentEconomy.AgentRuntime.Messaging.Abstractions;
using System;
using System.Collections.Concurrent;

namespace AiAgentEconomy.AgentRuntime.Messaging.InMemory
{
    public sealed class InMemoryMessageBus : IMessageBus
    {
        private readonly ConcurrentDictionary<Type, List<Func<object, CancellationToken, Task>>> _handlers = new();

        public Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class
        {
            if (_handlers.TryGetValue(typeof(T), out var handlers))
            {
                var tasks = handlers.Select(h => h(message, ct));
                return Task.WhenAll(tasks);
            }

            return Task.CompletedTask;
        }

        public void Subscribe<T>(Func<T, CancellationToken, Task> handler) where T : class
        {
            _handlers.AddOrUpdate(
                typeof(T),
                _ => [Wrap(handler)],
                (_, list) => { list.Add(Wrap(handler)); return list; }
            );
        }

        private static Func<object, CancellationToken, Task> Wrap<T>(Func<T, CancellationToken, Task> handler)
            where T : class
            => (obj, ct) => handler((T)obj, ct);
    }
}
