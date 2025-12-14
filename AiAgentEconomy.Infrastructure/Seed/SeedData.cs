using AiAgentEconomy.Domain.Marketplace;
using AiAgentEconomy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AiAgentEconomy.Infrastructure.Seed
{
    public static class SeedData
    {
        public static async Task ApplyAsync(AgentEconomyDbContext db, CancellationToken ct = default)
        {
            await db.Database.MigrateAsync(ct);

            // 1) Ensure Vendors
            var vendorA = await db.ServiceVendors
                .FirstOrDefaultAsync(x => x.Name == "VendorA", ct);

            if (vendorA is null)
            {
                vendorA = new ServiceVendor
                {
                    Name = "VendorA",
                    WalletAddress = "0x0000000000000000000000000000000000000001",
                    IsActive = true
                };
                db.ServiceVendors.Add(vendorA);
            }

            var vendorB = await db.ServiceVendors
                .FirstOrDefaultAsync(x => x.Name == "VendorB", ct);

            if (vendorB is null)
            {
                vendorB = new ServiceVendor
                {
                    Name = "VendorB",
                    WalletAddress = "0x0000000000000000000000000000000000000002",
                    IsActive = true
                };
                db.ServiceVendors.Add(vendorB);
            }

            await db.SaveChangesAsync(ct);

            // 2) Ensure Services (idempotent)
            await AddServiceIfMissing(db, vendorA.Id, "Service1", 5m, "USDC", ct);
            await AddServiceIfMissing(db, vendorA.Id, "Service2", 12m, "USDC", ct);
            await AddServiceIfMissing(db, vendorB.Id, "ComputeBasic", 3m, "USDC", ct);

            await db.SaveChangesAsync(ct);
        }

        private static async Task AddServiceIfMissing(
            AgentEconomyDbContext db,
            Guid vendorId,
            string serviceCode,
            decimal price,
            string currency,
            CancellationToken ct)
        {
            var exists = await db.MarketplaceServices.AnyAsync(
                s => s.VendorId == vendorId && s.ServiceCode == serviceCode,
                ct);

            if (exists) return;

            db.MarketplaceServices.Add(new MarketplaceService
            {
                VendorId = vendorId,
                ServiceCode = serviceCode,
                Price = price,
                Currency = currency,
                IsActive = true
            });
        }
    }
}
