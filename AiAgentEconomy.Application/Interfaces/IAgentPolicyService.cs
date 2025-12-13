using AiAgentEconomy.Contracts.Policies;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface IAgentPolicyService
    {
        Task<AgentPolicyDto?> GetByAgentIdAsync(Guid agentId, CancellationToken ct = default);
        Task<AgentPolicyDto> UpsertForAgentAsync(Guid agentId, UpsertAgentPolicyRequest request, CancellationToken ct = default);
    }
}
