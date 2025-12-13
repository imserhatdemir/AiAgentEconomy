using AiAgentEconomy.Domain.Agents.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface IAgentPolicyRepository
    {
        Task<AgentPolicy?> GetByAgentIdAsync(Guid agentId, CancellationToken ct = default);
        Task<AgentPolicy?> GetByAgentIdForUpdateAsync(Guid agentId, CancellationToken ct = default);
        Task AddAsync(AgentPolicy policy, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
