using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Agents;
using Microsoft.AspNetCore.Mvc;

namespace AiAgentEconomy.API.Controllers
{
    [Route("api/agents/{agentId:guid}/budget")]
    [ApiController]
    public class AgentBudgetController : ControllerBase
    {
        private readonly IAgentService _agentService;

        public AgentBudgetController(IAgentService agentService) => _agentService = agentService;

        [HttpPut]
        public async Task<IActionResult> Update(Guid agentId, [FromBody] UpdateAgentBudgetRequest request, CancellationToken ct)
        {
            await _agentService.UpdateMonthlyBudgetAsync(agentId, request, ct);
            return NoContent();
        }

        [HttpPost("reset-spent")]
        public async Task<IActionResult> ResetSpent(Guid agentId, [FromBody] ResetAgentSpentRequest request, CancellationToken ct)
        {
            await _agentService.ResetSpentThisMonthAsync(agentId, request, ct);
            return NoContent();
        }
    }
}
