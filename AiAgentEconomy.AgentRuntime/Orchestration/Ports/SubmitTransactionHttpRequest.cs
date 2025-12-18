
namespace AiAgentEconomy.AgentRuntime.Orchestration.Ports
{
    public sealed record SubmitTransactionHttpRequest(
        string Chain,
        string Network,
        string? ExplorerUrl
    );
}
