using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AiAgentEconomy.API.Controllers
{
    [Route("api/agents/{agentId:guid}/transactions")]
    [ApiController]
    public class AgentTransactionsController : ControllerBase
    {
        private readonly ITransactionService _txService;

        public AgentTransactionsController(ITransactionService txService) => _txService = txService;

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> Create(Guid agentId, [FromBody] CreateTransactionRequest request, CancellationToken ct)
        {
            var tx = await _txService.CreateForAgentAsync(agentId, request, ct);
            return CreatedAtAction(nameof(List), new { agentId }, tx);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TransactionDto>>> List(Guid agentId, [FromQuery] int take = 50, CancellationToken ct = default)
        {
            var list = await _txService.GetByAgentAsync(agentId, take, ct);
            return Ok(list);
        }
    }
}
