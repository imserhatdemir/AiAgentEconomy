namespace AiAgentEconomy.Contracts.Wallets
{
    public sealed record CreateWalletRequest(
        Guid UserId,
        string Address,
        string? Chain,
        string? Type
    );
}
