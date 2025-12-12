using AiAgentEconomy.Domain.Agents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Infrastructure.Persistence.Configurations
{
    public class AgentPolicyConfiguration : IEntityTypeConfiguration<AgentPolicy>
    {
        public void Configure(EntityTypeBuilder<AgentPolicy> builder)
        {
            builder.ToTable("agent_policies");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AgentId).IsRequired();

            builder.Property(x => x.MaxPerTransaction).HasColumnType("numeric(18,2)");
            builder.Property(x => x.DailyLimit).HasColumnType("numeric(18,2)");

            builder.Property(x => x.Currency).HasMaxLength(10).IsRequired();

            builder.Property(x => x.AllowedVendorsCsv).HasMaxLength(2000);
            builder.Property(x => x.AllowedServicesCsv).HasMaxLength(2000);

            builder.Property(x => x.SpentInDailyWindow).HasColumnType("numeric(18,2)");

            builder.Property(x => x.CreatedAtUtc).IsRequired();
            builder.Property(x => x.UpdatedAtUtc);

            builder.HasIndex(x => x.AgentId);
        }
    }
}
