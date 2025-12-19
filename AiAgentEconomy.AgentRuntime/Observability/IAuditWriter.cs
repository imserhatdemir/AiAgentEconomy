namespace AiAgentEconomy.AgentRuntime.Observability
{
    public interface IAuditWriter
    {
        Task WriteAsync(AuditRecord record, CancellationToken ct = default);
    }

    public sealed record AuditRecord(
        string EventType,
        string CorrelationId,
        string Actor,
        DateTimeOffset OccurredAt,
        object? Data = null
    );

}
