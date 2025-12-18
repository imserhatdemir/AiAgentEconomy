using System;
using System.Collections.Concurrent;

namespace AiAgentEconomy.AgentRuntime.Memory
{
    public sealed class InMemoryProcessedEventStore : IProcessedEventStore
    {
        private sealed record Entry(byte State, DateTimeOffset? LeaseUntil);
        private const byte InProgress = 1;
        private const byte Completed = 2;

        private readonly ConcurrentDictionary<string, Entry> _entries = new();

        public Task<BeginResult> TryBeginAsync(string eventKey, TimeSpan lease, CancellationToken ct = default)
        {
            var now = DateTimeOffset.UtcNow;

            while (true)
            {
                if (!_entries.TryGetValue(eventKey, out var existing))
                {
                    if (_entries.TryAdd(eventKey, new Entry(InProgress, now.Add(lease))))
                        return Task.FromResult(BeginResult.Begun);

                    continue; // yarış oldu, tekrar dene
                }

                if (existing.State == Completed)
                    return Task.FromResult(BeginResult.AlreadyCompleted);

                // InProgress ama lease bitmiş mi?
                if (existing.LeaseUntil.HasValue && existing.LeaseUntil.Value <= now)
                {
                    // Lease expired -> take over
                    if (_entries.TryUpdate(eventKey, new Entry(InProgress, now.Add(lease)), existing))
                        return Task.FromResult(BeginResult.Begun);

                    continue; // yarış oldu
                }

                return Task.FromResult(BeginResult.AlreadyInProgress);
            }
        }

        public Task MarkCompletedAsync(string eventKey, CancellationToken ct = default)
        {
            _entries.AddOrUpdate(
                eventKey,
                _ => new Entry(Completed, null),
                (_, __) => new Entry(Completed, null)
            );
            return Task.CompletedTask;
        }
    }
}
