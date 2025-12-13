using AiAgentEconomy.Domain.Wallets;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface IWalletRepository
    {
        Task AddAsync(Wallet wallet, CancellationToken ct = default);
        Task<Wallet?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Wallet?> GetByAddressAsync(string address, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
