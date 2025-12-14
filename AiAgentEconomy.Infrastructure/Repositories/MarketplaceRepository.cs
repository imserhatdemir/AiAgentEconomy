using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Marketplace;
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
        => _db.ServiceVendors
              .AsNoTracking()
              .FirstOrDefaultAsync(v => v.Name == name, ct);

        public Task<MarketplaceService?> GetServiceAsync(Guid vendorId, string serviceCode, CancellationToken ct = default)
            => _db.MarketplaceServices.AsNoTracking()
                .FirstOrDefaultAsync(x => x.VendorId == vendorId && x.ServiceCode == serviceCode, ct);

        public Task<IReadOnlyList<ServiceVendor>> GetVendorsAsync(bool onlyActive, CancellationToken ct = default)
        {
            var q = _db.ServiceVendors.AsNoTracking();
            if (onlyActive) q = q.Where(x => x.IsActive);
            return q.OrderBy(x => x.Name).ToListAsync(ct).ContinueWith(t => (IReadOnlyList<ServiceVendor>)t.Result, ct);
        }
        
        public Task<ServiceVendor?> GetVendorByIdAsync(Guid id, CancellationToken ct = default)
         => _db.ServiceVendors.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        
        public Task<IReadOnlyList<MarketplaceService>> GetServicesByVendorIdAsync(Guid vendorId, bool onlyActive, CancellationToken ct = default)
        {
            var q = _db.MarketplaceServices.AsNoTracking().Where(x => x.VendorId == vendorId);
            if (onlyActive) q = q.Where(x => x.IsActive);
            return q.OrderBy(x => x.ServiceCode).ToListAsync(ct).ContinueWith(t => (IReadOnlyList<MarketplaceService>)t.Result, ct);
        }
        
        public Task<MarketplaceService?> GetActiveServiceAsync(Guid vendorId, string serviceCode, CancellationToken ct = default)
        => _db.MarketplaceServices
              .AsNoTracking()
              .FirstOrDefaultAsync(s =>
                    s.VendorId == vendorId &&
                    s.ServiceCode == serviceCode &&
                    s.IsActive, ct);
    }
}
