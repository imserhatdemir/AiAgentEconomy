using AiAgentEconomy.Contracts.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface IAgentService
    {
        Task<AgentDto> CreateAsync(CreateAgentRequest request, CancellationToken ct = default);
        Task<AgentDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<AgentDto>> GetAllAsync(CancellationToken ct = default);
    }
}
