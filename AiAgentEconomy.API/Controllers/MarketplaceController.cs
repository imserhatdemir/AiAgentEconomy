using AiAgentEconomy.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AiAgentEconomy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketplaceController : ControllerBase
    {
        private readonly IMarketplaceReadService _service;

        public MarketplaceController(IMarketplaceReadService service)
        {
            _service = service;
        }

        [HttpGet("vendors")]
        public async Task<IActionResult> GetVendors([FromQuery] bool onlyActive = true, CancellationToken ct = default)
            => Ok(await _service.GetVendorsAsync(onlyActive, ct));

        [HttpGet("vendors/{vendorId:guid}")]
        public async Task<IActionResult> GetVendorById(Guid vendorId, CancellationToken ct = default)
            => Ok(await _service.GetVendorByIdAsync(vendorId, ct));

        [HttpGet("vendors/{vendorId:guid}/services")]
        public async Task<IActionResult> GetServicesByVendor(Guid vendorId, [FromQuery] bool onlyActive = true, CancellationToken ct = default)
            => Ok(await _service.GetServicesByVendorAsync(vendorId, onlyActive, ct));

        // Optional helper endpoint
        [HttpGet("services/find")]
        public async Task<IActionResult> FindService([FromQuery] string vendor, [FromQuery] string serviceCode, CancellationToken ct = default)
            => Ok(await _service.FindServiceAsync(vendor, serviceCode, ct));
    }
}
