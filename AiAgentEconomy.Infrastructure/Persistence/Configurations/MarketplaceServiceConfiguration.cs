using AiAgentEconomy.Domain.Marketplace;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Infrastructure.Persistence.Configurations
{
    public class MarketplaceServiceConfiguration : IEntityTypeConfiguration<MarketplaceService>
    {
        public void Configure(EntityTypeBuilder<MarketplaceService> builder)
        {
            builder.ToTable("marketplace_services");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ServiceCode).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Currency).HasMaxLength(16).IsRequired();

            builder.Property(x => x.Price).HasPrecision(18, 2).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();

            builder.HasIndex(x => new { x.VendorId, x.ServiceCode }).IsUnique();
            builder.HasIndex(x => x.ServiceCode);
        }
    }
}
