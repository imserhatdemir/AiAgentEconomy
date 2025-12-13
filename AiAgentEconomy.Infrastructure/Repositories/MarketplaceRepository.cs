using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Domain.Marketplace;
using AiAgentEconomy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AiAgentEconomy.Infrastructure.Repositories
{
    public sealed class MarketplaceRepository : IMarketplaceRepository
    {
        private readonly AgentEconomyDbContext _db;

        public MarketplaceRepository(AgentEconomyDbContext db) => _db = db;

        public Task<ServiceVendor?> GetVendorByNameAsync(string name, CancellationToken ct = default)
            => _db.ServiceVendors.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name, ct);

        public Task<MarketplaceService?> GetServiceAsync(Guid vendorId, string serviceCode, CancellationToken ct = default)
            => _db.MarketplaceServices.AsNoTracking()
                .FirstOrDefaultAsync(x => x.VendorId == vendorId && x.ServiceCode == serviceCode, ct);
    }
}
