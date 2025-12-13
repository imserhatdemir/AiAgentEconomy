using AiAgentEconomy.Domain.Marketplace;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface IMarketplaceRepository
    {
        Task<ServiceVendor?> GetVendorByNameAsync(string name, CancellationToken ct = default);
        Task<MarketplaceService?> GetServiceAsync(Guid vendorId, string serviceCode, CancellationToken ct = default);
    }
}
