using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace AiAgentEconomy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _txService;

        public TransactionsController(ITransactionService txService)
        {
            _txService = txService;
        }

        [HttpPost("{transactionId:guid}/submit")]
        public async Task<ActionResult<TransactionDto>> Submit(
            Guid transactionId,
            [FromBody] SubmitTransactionRequest request,
            CancellationToken ct)
        {
            var result = await _txService.SubmitAsync(transactionId, request, ct);
            return Ok(result);
        }

        [HttpPost("{transactionId:guid}/settle")]
        public async Task<ActionResult<TransactionDto>> Settle(
            Guid transactionId,
            CancellationToken ct)
        {
            var result = await _txService.SettleAsync(transactionId, ct);
            return Ok(result);
        }

        [HttpPost("{transactionId:guid}/fail")]
        public async Task<ActionResult<TransactionDto>> Fail(
            Guid transactionId,
            [FromBody] FailTransactionRequest request,
            CancellationToken ct)
        {
            var result = await _txService.FailAsync(transactionId, request, ct);
            return Ok(result);
        }
    }
}
