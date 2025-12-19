namespace AiAgentEconomy.AgentRuntime.Messaging.Contracts
{
    public sealed record TransactionApproved(
        Guid TransactionId,
        Guid AgentId,
        decimal Amount,
        string Currency,
        string CorrelationId,
        DateTimeOffset OccurredAt
    );
}
