using AiAgentEconomy.Domain.Marketplace;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface IMarketplaceRepository
    {
        Task<ServiceVendor?> GetVendorByNameAsync(string name, CancellationToken ct = default);
        Task<MarketplaceService?> GetServiceAsync(Guid vendorId, string serviceCode, CancellationToken ct = default);
        Task<IReadOnlyList<ServiceVendor>> GetVendorsAsync(bool onlyActive, CancellationToken ct = default);
        Task<ServiceVendor?> GetVendorByIdAsync(Guid id, CancellationToken ct = default);

        Task<IReadOnlyList<MarketplaceService>> GetServicesByVendorIdAsync(Guid vendorId, bool onlyActive, CancellationToken ct = default);

    }
}
