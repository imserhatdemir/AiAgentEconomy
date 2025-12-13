using AiAgentEconomy.Application.Exceptions;
using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Policies;
using AiAgentEconomy.Domain.Agents.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Application.Services
{
    public sealed class AgentPolicyService : IAgentPolicyService
    {
        private readonly IAgentRepository _agentRepo;
        private readonly IAgentPolicyRepository _policyRepo;

        public AgentPolicyService(IAgentRepository agentRepo, IAgentPolicyRepository policyRepo)
        {
            _agentRepo = agentRepo;
            _policyRepo = policyRepo;
        }

        public async Task<AgentPolicyDto?> GetByAgentIdAsync(Guid agentId, CancellationToken ct = default)
        {
            var p = await _policyRepo.GetByAgentIdAsync(agentId, ct);
            return p is null ? null : ToDto(p);
        }

        public async Task<AgentPolicyDto> UpsertForAgentAsync(Guid agentId, UpsertAgentPolicyRequest request, CancellationToken ct = default)
        {
            // Agent exists?
            var agent = await _agentRepo.GetByIdAsync(agentId, ct);
            if (agent is null)
                throw new NotFoundException("Agent not found.");

            if (request.MaxPerTransaction < 0)
                throw new ValidationException("MaxPerTransaction cannot be negative.");

            if (request.DailyLimit < 0)
                throw new ValidationException("DailyLimit cannot be negative.");

            var currency = string.IsNullOrWhiteSpace(request.Currency) ? "USDC" : request.Currency.Trim();

            var policy = await _policyRepo.GetByAgentIdForUpdateAsync(agentId, ct);

            if (policy is null)
            {
                policy = new AgentPolicy
                {
                    AgentId = agentId,
                    Name = string.IsNullOrWhiteSpace(request.Name) ? "Default Policy" : request.Name.Trim(),
                    IsActive = request.IsActive,
                    MaxPerTransaction = request.MaxPerTransaction,
                    DailyLimit = request.DailyLimit,
                    Currency = currency,
                    AllowedVendorsCsv = request.AllowedVendorsCsv,
                    AllowedServicesCsv = request.AllowedServicesCsv
                };

                await _policyRepo.AddAsync(policy, ct);
            }
            else
            {
                policy.Name = string.IsNullOrWhiteSpace(request.Name) ? policy.Name : request.Name.Trim();
                policy.IsActive = request.IsActive;
                policy.MaxPerTransaction = request.MaxPerTransaction;
                policy.DailyLimit = request.DailyLimit;
                policy.Currency = currency;
                policy.AllowedVendorsCsv = request.AllowedVendorsCsv;
                policy.AllowedServicesCsv = request.AllowedServicesCsv;
                policy.UpdatedAtUtc = DateTime.UtcNow;
            }

            await _policyRepo.SaveChangesAsync(ct);

            return ToDto(policy);
        }

        private static AgentPolicyDto ToDto(AgentPolicy p)
            => new(
                p.Id,
                p.AgentId,
                p.Name,
                p.IsActive,
                p.MaxPerTransaction,
                p.DailyLimit,
                p.Currency,
                p.AllowedVendorsCsv,
                p.AllowedServicesCsv,
                p.DailyWindowDate,
                p.SpentInDailyWindow,
                p.CreatedAtUtc
            );
    }
}
