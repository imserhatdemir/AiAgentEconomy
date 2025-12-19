namespace AiAgentEconomy.AgentRuntime.Policies
{
    public sealed class DefaultPolicyEvaluator : IPolicyEvaluator
    {
        public Task<PolicyDecision> EvaluateAsync(PolicyContext ctx, CancellationToken ct = default)
            => Task.FromResult(new PolicyDecision(true));
    }
}
