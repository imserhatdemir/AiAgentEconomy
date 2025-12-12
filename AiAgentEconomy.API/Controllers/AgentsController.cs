using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Agents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AiAgentEconomy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentService _service;

        public AgentsController(IAgentService service) => _service = service;

        [HttpPost]
        public async Task<ActionResult<AgentDto>> Create([FromBody] CreateAgentRequest request, CancellationToken ct)
        {
            var created = await _service.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AgentDto>> GetById(Guid id, CancellationToken ct)
        {
            var agent = await _service.GetByIdAsync(id, ct);
            return agent is null ? NotFound() : Ok(agent);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AgentDto>>> GetAll(CancellationToken ct)
            => Ok(await _service.GetAllAsync(ct));
    }
}
