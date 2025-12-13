using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Domain.Agents;
using AiAgentEconomy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Infrastructure.Repositories
{
    public sealed class AgentRepository : IAgentRepository
    {
        private readonly AgentEconomyDbContext _db;

        public AgentRepository(AgentEconomyDbContext db) => _db = db;

        public Task AddAsync(Agent agent, CancellationToken ct = default)
            => _db.Agents.AddAsync(agent, ct).AsTask();

        public Task<Agent?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => _db.Agents.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task<List<Agent>> GetAllAsync(CancellationToken ct = default)
            => _db.Agents.AsNoTracking().OrderByDescending(x => x.CreatedAtUtc).ToListAsync(ct);

        public Task<Agent?> GetByIdForUpdateAsync(Guid id, CancellationToken ct = default)
            => _db.Agents.FirstOrDefaultAsync(x => x.Id == id, ct); // AsNoTracking yok

        public Task SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);
    }
}
