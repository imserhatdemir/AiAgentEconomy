using AiAgentEconomy.Contracts.Wallets;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface IWalletService
    {
        Task<WalletDto> CreateAsync(CreateWalletRequest request, CancellationToken ct = default);
        Task<WalletDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
