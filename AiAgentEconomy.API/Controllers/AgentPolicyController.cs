using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Policies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AiAgentEconomy.API.Controllers
{
    [Route("api/agents/{agentId:guid}/policy")]
    [ApiController]
    public class AgentPolicyController : ControllerBase
    {
        private readonly IAgentPolicyService _policyService;

        public AgentPolicyController(IAgentPolicyService policyService)
            => _policyService = policyService;

        [HttpGet]
        public async Task<ActionResult<AgentPolicyDto>> Get(Guid agentId, CancellationToken ct)
        {
            var policy = await _policyService.GetByAgentIdAsync(agentId, ct);
            return policy is null ? NotFound() : Ok(policy);
        }

        [HttpPut]
        public async Task<ActionResult<AgentPolicyDto>> Upsert(Guid agentId, [FromBody] UpsertAgentPolicyRequest request, CancellationToken ct)
        {
            var policy = await _policyService.UpsertForAgentAsync(agentId, request, ct);
            return Ok(policy);
        }
    }
}
