namespace AiAgentEconomy.Contracts.Wallets
{
    public sealed record CreateWalletRequest(
        Guid AgentId,
        string Address,
        string? Chain,
        string? Type
    );
}
