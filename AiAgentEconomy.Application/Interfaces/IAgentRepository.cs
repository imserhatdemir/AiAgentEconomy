using AiAgentEconomy.Domain.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface IAgentRepository
    {
        Task AddAsync(Agent agent, CancellationToken ct = default);
        Task<Agent?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<Agent>> GetAllAsync(CancellationToken ct = default);
        Task<Agent?> GetByIdForUpdateAsync(Guid id, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
