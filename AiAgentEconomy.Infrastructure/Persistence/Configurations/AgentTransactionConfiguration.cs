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
    public class AgentTransactionConfiguration : IEntityTypeConfiguration<AgentTransaction>
    {
        public void Configure(EntityTypeBuilder<AgentTransaction> builder)
        {
            builder.ToTable("agent_transactions");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AgentId).IsRequired();
            builder.Property(x => x.WalletId).IsRequired();
            builder.Property(x => x.ServiceVendorId).IsRequired();

            builder.Property(x => x.Amount).HasColumnType("numeric(18,2)");
            builder.Property(x => x.Currency).HasMaxLength(10).IsRequired();

            builder.Property(x => x.BlockchainTxHash).HasMaxLength(128);

            builder.Property(x => x.Status).IsRequired();

            builder.Property(x => x.MetadataJson).HasColumnType("text");

            builder.Property(x => x.CreatedAtUtc).IsRequired();
            builder.Property(x => x.UpdatedAtUtc);

            builder.HasIndex(x => x.AgentId);
            builder.HasIndex(x => x.Status);
        }
    }
}
