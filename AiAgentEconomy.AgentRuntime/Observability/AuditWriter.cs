namespace AiAgentEconomy.AgentRuntime.Observability
{
    public sealed class AuditWriter(ILogger<AuditWriter> logger) : IAuditWriter
    {
        public Task WriteAsync(AuditRecord record, CancellationToken ct = default)
        {
            logger.LogInformation("AUDIT {EventType} CorrelationId={CorrelationId} Actor={Actor} At={At} Data={Data}",
                record.EventType, record.CorrelationId, record.Actor, record.OccurredAt, record.Data);
            return Task.CompletedTask;
        }
    }
}
