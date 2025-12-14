using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Domain.Transactions;
using AiAgentEconomy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AiAgentEconomy.Infrastructure.Repositories
{
    public sealed class TransactionRepository : ITransactionRepository
    {
        private readonly AgentEconomyDbContext _db;
        public TransactionRepository(AgentEconomyDbContext db) => _db = db;

        public Task AddAsync(Transaction tx, CancellationToken ct = default)
            => _db.Transactions.AddAsync(tx, ct).AsTask();

        public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => _db.Transactions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<IReadOnlyList<Transaction>> GetByAgentIdAsync(Guid agentId, int take = 50, CancellationToken ct = default)
            => await _db.Transactions.AsNoTracking()
                .Where(x => x.AgentId == agentId)
                .OrderByDescending(x => x.CreatedAtUtc)
                .Take(take)
                .ToListAsync(ct);

        public Task SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);
        public Task<Transaction?> GetByIdForUpdateAsync(Guid id, CancellationToken ct = default)
            => _db.Transactions.FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
