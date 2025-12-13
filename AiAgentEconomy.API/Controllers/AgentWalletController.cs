using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Wallets;
using Microsoft.AspNetCore.Mvc;

namespace AiAgentEconomy.API.Controllers
{
    [Route("api/agents/{agentId:guid}/wallet")]
    [ApiController]
    public class AgentWalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public AgentWalletController(IWalletService walletService)
            => _walletService = walletService;

        [HttpPost]
        public async Task<ActionResult<WalletDto>> Create(Guid agentId, [FromBody] CreateWalletForAgentRequest request, CancellationToken ct)
        {
            var wallet = await _walletService.CreateForAgentAsync(agentId, request, ct);
            return CreatedAtAction(nameof(Get), new { agentId }, wallet);
        }

        [HttpGet]
        public async Task<ActionResult<WalletDto>> Get(Guid agentId, CancellationToken ct)
        {
            var wallet = await _walletService.GetByAgentIdAsync(agentId, ct);
            return wallet is null ? NotFound() : Ok(wallet);
        }
    }
}
