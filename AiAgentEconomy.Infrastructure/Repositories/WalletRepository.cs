using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Domain.Wallets;
using AiAgentEconomy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AiAgentEconomy.Infrastructure.Repositories
{
    public sealed class WalletRepository : IWalletRepository
    {
        private readonly AgentEconomyDbContext _db;

        public WalletRepository(AgentEconomyDbContext db) => _db = db;

        public Task AddAsync(Wallet wallet, CancellationToken ct = default)
            => _db.Wallets.AddAsync(wallet, ct).AsTask();

        public Task<Wallet?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => _db.Wallets.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task<Wallet?> GetByAgentIdAsync(Guid agentId, CancellationToken ct = default)
            => _db.Wallets.AsNoTracking().FirstOrDefaultAsync(x => x.AgentId == agentId, ct);

        public Task<bool> ExistsByChainAndAddressAsync(string chain, string address, CancellationToken ct = default)
            => _db.Wallets.AsNoTracking().AnyAsync(x => x.Chain == chain && x.Address == address, ct);

        public Task SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);
    }
}
