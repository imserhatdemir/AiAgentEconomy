using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Domain.Agents.Policies;
using AiAgentEconomy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AiAgentEconomy.Infrastructure.Repositories
{
    public sealed class AgentPolicyRepository : IAgentPolicyRepository
    {
        private readonly AgentEconomyDbContext _db;
        public AgentPolicyRepository(AgentEconomyDbContext db) => _db = db;

        public Task<AgentPolicy?> GetByAgentIdAsync(Guid agentId, CancellationToken ct = default)
            => _db.AgentPolicies.AsNoTracking().FirstOrDefaultAsync(x => x.AgentId == agentId, ct);

        public Task<AgentPolicy?> GetByAgentIdForUpdateAsync(Guid agentId, CancellationToken ct = default)
            => _db.AgentPolicies.FirstOrDefaultAsync(x => x.AgentId == agentId, ct);

        public Task AddAsync(AgentPolicy policy, CancellationToken ct = default)
            => _db.AgentPolicies.AddAsync(policy, ct).AsTask();

        public Task SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);
    }
}
