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
    public class ServiceVendorConfiguration : IEntityTypeConfiguration<ServiceVendor>
    {
        public void Configure(EntityTypeBuilder<ServiceVendor> builder)
        {
            builder.ToTable("service_vendors");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Category).HasMaxLength(100).IsRequired();

            builder.Property(x => x.WalletAddress).HasMaxLength(128).IsRequired();

            builder.Property(x => x.Price).HasColumnType("numeric(18,2)");
            builder.Property(x => x.Currency).HasMaxLength(10).IsRequired();

            builder.Property(x => x.IsActive).IsRequired();

            builder.Property(x => x.CreatedAtUtc).IsRequired();
            builder.Property(x => x.UpdatedAtUtc);

            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.IsActive);
        }
    }
}
