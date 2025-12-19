namespace AiAgentEconomy.AgentRuntime.Policies
{
    public interface IPolicyEvaluator
    {
        Task<PolicyDecision> EvaluateAsync(PolicyContext ctx, CancellationToken ct = default);
    }

    public sealed record PolicyContext(Guid AgentId, Guid TransactionId, decimal Amount, string Currency);

    public sealed record PolicyDecision(bool Allowed, string? Reason = null);
}
