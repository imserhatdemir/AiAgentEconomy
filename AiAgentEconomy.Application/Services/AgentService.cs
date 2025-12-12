using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Agents;
using AiAgentEconomy.Domain.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Application.Services
{
    public sealed class AgentService : IAgentService
    {
        private readonly IAgentRepository _repo;

        public AgentService(IAgentRepository repo) => _repo = repo;

        public async Task<AgentDto> CreateAsync(CreateAgentRequest request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Name is required.", nameof(request.Name));

            var agent = new Agent
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                CreatedAtUtc = DateTime.UtcNow
            };

            await _repo.AddAsync(agent, ct);
            await _repo.SaveChangesAsync(ct);

            return new AgentDto(agent.Id, agent.Name, agent.Description, agent.CreatedAtUtc);
        }

        public async Task<AgentDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var agent = await _repo.GetByIdAsync(id, ct);
            return agent is null ? null : new AgentDto(agent.Id, agent.Name, agent.Description, agent.CreatedAtUtc);
        }

        public async Task<IReadOnlyList<AgentDto>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _repo.GetAllAsync(ct);
            return list.Select(x => new AgentDto(x.Id, x.Name, x.Description, x.CreatedAtUtc)).ToList();
        }
    }
}
