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
    public class AgentConfiguration : IEntityTypeConfiguration<Agent>
    {
        public void Configure(EntityTypeBuilder<Agent> builder)
        {
            builder.ToTable("agents");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Goal).HasMaxLength(2000).IsRequired();

            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.RiskLevel).IsRequired();

            builder.Property(x => x.MonthlyBudget).HasColumnType("numeric(18,2)");
            builder.Property(x => x.SpentThisMonth).HasColumnType("numeric(18,2)");

            builder.Property(x => x.CreatedAtUtc).IsRequired();
            builder.Property(x => x.UpdatedAtUtc);

            builder.HasOne(a => a.Wallet)
                   .WithOne()
                   .HasForeignKey<Wallet>(w => w.AgentId);

            builder.HasOne(x => x.Policy)
                   .WithMany()
                   .HasForeignKey(x => x.PolicyId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Status);
        }
    }
}
