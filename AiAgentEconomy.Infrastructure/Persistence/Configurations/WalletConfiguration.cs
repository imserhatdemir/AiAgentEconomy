using AiAgentEconomy.Domain.Agents;
using AiAgentEconomy.Domain.Wallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Infrastructure.Persistence.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("wallets");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AgentId).IsRequired();

            builder.Property(x => x.Chain).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Address).HasMaxLength(128).IsRequired();

            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();

            builder.Property(x => x.CreatedAtUtc).IsRequired();
            builder.Property(x => x.UpdatedAtUtc);
            builder.HasOne<Agent>()
                                   .WithOne(a => a.Wallet)
                                   .HasForeignKey<Wallet>(w => w.AgentId)
                                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.AgentId);
            builder.HasIndex(x => x.Address).IsUnique();
        }
    }
}
