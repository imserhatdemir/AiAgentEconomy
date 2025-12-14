using AiAgentEconomy.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Infrastructure.Persistence.Configurations
{
    public sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("transactions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AgentId).IsRequired();
            builder.Property(x => x.WalletId).IsRequired();

            builder.Property(x => x.Amount)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(x => x.Currency)
                   .HasMaxLength(16)
                   .IsRequired();

            builder.Property(x => x.Type)
                   .IsRequired();

            builder.Property(x => x.Status)
                   .IsRequired();

            builder.Property(x => x.RejectionReason)
                   .HasMaxLength(256);

            builder.Property(x => x.CreatedAtUtc).IsRequired();
            builder.Property(x => x.UpdatedAtUtc);
            builder.Property(x => x.Vendor).HasMaxLength(128);
            builder.Property(x => x.ServiceCode).HasMaxLength(128);
            builder.Property(x => x.VendorId).IsRequired(false);
            builder.Property(x => x.MarketplaceServiceId).IsRequired(false);
            builder.Property(x => x.UnitPrice)
                   .HasColumnType("numeric(18,2)")
                   .IsRequired(false);

            builder.Property(x => x.UnitPriceCurrency)
                   .HasMaxLength(16)
                   .IsRequired(false);
            builder.Property(x => x.Chain).HasMaxLength(64);
            builder.Property(x => x.Network).HasMaxLength(64);
            builder.Property(x => x.BlockchainTxHash).HasMaxLength(128);
            builder.Property(x => x.ExplorerUrl).HasMaxLength(512);

            builder.Property(x => x.FailureReason).HasMaxLength(256);

            builder.HasIndex(x => x.BlockchainTxHash);
            builder.HasIndex(x => x.Status);

            // Query patterns:
            // - List transactions by AgentId
            // - List transactions by WalletId
            // - Poll pending/approved tx
            builder.HasIndex(x => x.AgentId);
            builder.HasIndex(x => x.WalletId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.Vendor);
            builder.HasIndex(x => x.ServiceCode);
            builder.HasIndex(x => x.VendorId);
            builder.HasIndex(x => x.MarketplaceServiceId);
            // Optional composite index for runtime polling
            builder.HasIndex(x => new { x.Status, x.CreatedAtUtc });
        }
    }
}
