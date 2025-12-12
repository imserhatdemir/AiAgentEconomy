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
                db.ServiceVendors.AddRange(
                    new ServiceVendor
                    {
                        Name = "DataAPI.io",
                        Category = "Data API",
                        WalletAddress = "0x0000000000000000000000000000000000000001",
                        Price = 5m,
                        Currency = "USDC",
                        IsActive = true
                    },
                    new ServiceVendor
                    {
                        Name = "ComputeSaaS",
                        Category = "SaaS",
                        WalletAddress = "0x0000000000000000000000000000000000000002",
                        Price = 12m,
                        Currency = "USDC",
                        IsActive = true
                    },
                    new ServiceVendor
                    {
                        Name = "MarketSignals",
                        Category = "Analytics",
                        WalletAddress = "0x0000000000000000000000000000000000000003",
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
