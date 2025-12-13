namespace AiAgentEconomy.Contracts.Wallets
{
    public sealed record WalletDto(
        Guid Id,
        Guid UserId,
        string Chain,
        string Address,
        string Type,
        bool IsActive,
        DateTimeOffset CreatedAtUtc
    );
}
