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

            if (!await db.ServiceVendors.AnyAsync(ct))
            {
                var vendorA = new ServiceVendor
                {
                    Name = "VendorA",
                    WalletAddress = "0x0000000000000000000000000000000000000001",
                    IsActive = true
                };

                var vendorB = new ServiceVendor
                {
                    Name = "VendorB",
                    WalletAddress = "0x0000000000000000000000000000000000000002",
                    IsActive = true
                };

                db.ServiceVendors.AddRange(vendorA, vendorB);
                await db.SaveChangesAsync(ct);

                db.MarketplaceServices.AddRange(
                    new MarketplaceService
                    {
                        VendorId = vendorA.Id,
                        ServiceCode = "Service1",
                        Price = 5m,
                        Currency = "USDC",
                        IsActive = true
                    },
                    new MarketplaceService
                    {
                        VendorId = vendorA.Id,
                        ServiceCode = "Service2",
                        Price = 12m,
                        Currency = "USDC",
                        IsActive = true
                    },
                    new MarketplaceService
                    {
                        VendorId = vendorB.Id,
                        ServiceCode = "ComputeBasic",
                        Price = 3m,
                        Currency = "USDC",
                        IsActive = true
                    }
                );

                await db.SaveChangesAsync(ct);
            }

        }
    }
}
