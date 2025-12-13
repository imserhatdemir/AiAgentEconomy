using AiAgentEconomy.Domain.Agents.Policies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiAgentEconomy.Infrastructure.Persistence.Configurations
{
    public class AgentPolicyConfiguration : IEntityTypeConfiguration<AgentPolicy>
    {
        public void Configure(EntityTypeBuilder<AgentPolicy> builder)
        {
            builder.ToTable("agent_policies");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AgentId).IsRequired();

            builder.Property(x => x.Name)
                   .HasMaxLength(128)
                   .IsRequired();

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.Property(x => x.Currency)
                   .HasMaxLength(16)
                   .IsRequired();

            builder.Property(x => x.MaxPerTransaction)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(x => x.DailyLimit)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(x => x.AllowedVendorsCsv)
                   .HasMaxLength(512);

            builder.Property(x => x.AllowedServicesCsv)
                   .HasMaxLength(512);

            // DateOnly mapping (EF Core 8/9 Npgsql supports DateOnly)
            builder.Property(x => x.DailyWindowDate);

            builder.Property(x => x.SpentInDailyWindow)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(x => x.CreatedAtUtc).IsRequired();
            builder.Property(x => x.UpdatedAtUtc);

            builder.HasIndex(x => x.AgentId).IsUnique();   // Agent -> Policy 1-1 (her agent 1 policy)
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.Name);
        }
    }
}
