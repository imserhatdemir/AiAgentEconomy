using AiAgentEconomy.Application.Exceptions;
using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Marketplace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Application.Services
{
    public sealed class MarketplaceReadService : IMarketplaceReadService
    {
        private readonly IMarketplaceRepository _repo;

        public MarketplaceReadService(IMarketplaceRepository repo)
        {
            _repo = repo;
        }

        public async Task<IReadOnlyList<ServiceVendorDto>> GetVendorsAsync(bool onlyActive = true, CancellationToken ct = default)
        {
            var vendors = await _repo.GetVendorsAsync(onlyActive, ct);
            return vendors.Select(ToDto).ToList();
        }

        public async Task<ServiceVendorDto> GetVendorByIdAsync(Guid vendorId, CancellationToken ct = default)
        {
            var vendor = await _repo.GetVendorByIdAsync(vendorId, ct);
            if (vendor is null)
                throw new NotFoundException("Vendor not found.");

            return ToDto(vendor);
        }

        public async Task<IReadOnlyList<MarketplaceServiceDto>> GetServicesByVendorAsync(Guid vendorId, bool onlyActive = true, CancellationToken ct = default)
        {
            // vendor existence gate (nice UX)
            var vendor = await _repo.GetVendorByIdAsync(vendorId, ct);
            if (vendor is null)
                throw new NotFoundException("Vendor not found.");

            var services = await _repo.GetServicesByVendorIdAsync(vendorId, onlyActive, ct);
            return services.Select(ToDto).ToList();
        }

        public async Task<MarketplaceServiceDto?> FindServiceAsync(string vendorName, string serviceCode, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(vendorName) || string.IsNullOrWhiteSpace(serviceCode))
                return null;

            var vendor = await _repo.GetVendorByNameAsync(vendorName.Trim(), ct);
            if (vendor is null)
                return null;

            var svc = await _repo.GetServiceAsync(vendor.Id, serviceCode.Trim(), ct);
            return svc is null ? null : ToDto(svc);
        }

        private static ServiceVendorDto ToDto(Domain.Marketplace.ServiceVendor v)
            => new(v.Id, v.Name, v.Category, v.WalletAddress, v.IsActive, v.CreatedAtUtc);

        private static MarketplaceServiceDto ToDto(Domain.Marketplace.MarketplaceService s)
            => new(s.Id, s.VendorId, s.ServiceCode, s.Name, s.Price, s.Currency, s.IsActive, s.CreatedAtUtc);
    }
}
