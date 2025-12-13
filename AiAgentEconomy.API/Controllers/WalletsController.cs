using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Wallets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AiAgentEconomy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _service;
        public WalletsController(IWalletService service) => _service = service;

        [HttpPost]
        public async Task<ActionResult<WalletDto>> Create([FromBody] CreateWalletRequest request, CancellationToken ct)
        {
            var created = await _service.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<WalletDto>> GetById(Guid id, CancellationToken ct)
        {
            var wallet = await _service.GetByIdAsync(id, ct);
            return wallet is null ? NotFound() : Ok(wallet);
        }
    }
}
