using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace AiAgentEconomy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionLifecycleService _lifecycle;

        public TransactionsController(ITransactionLifecycleService lifecycle)
        {
            _lifecycle = lifecycle;
        }

        [HttpPost("{id:guid}/submit")]
        public async Task<ActionResult<TransactionDto>> Submit(Guid id, [FromBody] SubmitTransactionRequest request, CancellationToken ct)
        {
            var result = await _lifecycle.SubmitAsync(id, request, ct);
            return Ok(result);
        }

        [HttpPost("{id:guid}/settle")]
        public async Task<ActionResult<TransactionDto>> Settle(Guid id, CancellationToken ct)
        {
            var result = await _lifecycle.SettleAsync(id, ct);
            return Ok(result);
        }

        [HttpPost("{id:guid}/fail")]
        public async Task<ActionResult<TransactionDto>> Fail(Guid id, [FromBody] FailTransactionRequest request, CancellationToken ct)
        {
            var result = await _lifecycle.FailAsync(id, request.Reason, ct);
            return Ok(result);
        }
    }
}
