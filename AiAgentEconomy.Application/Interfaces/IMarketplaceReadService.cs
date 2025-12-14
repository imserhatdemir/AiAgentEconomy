using AiAgentEconomy.Contracts.Marketplace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface IMarketplaceReadService
    {
        Task<IReadOnlyList<ServiceVendorDto>> GetVendorsAsync(bool onlyActive = true, CancellationToken ct = default);
        Task<ServiceVendorDto> GetVendorByIdAsync(Guid vendorId, CancellationToken ct = default);

        Task<IReadOnlyList<MarketplaceServiceDto>> GetServicesByVendorAsync(Guid vendorId, bool onlyActive = true, CancellationToken ct = default);

        // Optional: vendor name + serviceCode quick lookup for UI/debug
        Task<MarketplaceServiceDto?> FindServiceAsync(string vendorName, string serviceCode, CancellationToken ct = default);
    }
}
