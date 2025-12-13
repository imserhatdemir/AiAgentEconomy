using AiAgentEconomy.Domain.Transactions;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction tx, CancellationToken ct = default);
        Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<Transaction>> GetByAgentIdAsync(Guid agentId, int take = 50, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
