using AiAgentEconomy.Domain.Agents;
using AiAgentEconomy.Domain.Marketplace;
using AiAgentEconomy.Domain.Transactions;
using AiAgentEconomy.Domain.Wallets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Infrastructure.Persistence
{
    public class AgentEconomyDbContext : DbContext
    {
        public AgentEconomyDbContext(DbContextOptions<AgentEconomyDbContext> options) : base(options) { }

        public DbSet<Agent> Agents => Set<Agent>();
        public DbSet<AgentPolicy> AgentPolicies => Set<AgentPolicy>();
        public DbSet<Wallet> Wallets => Set<Wallet>();
        public DbSet<ServiceVendor> ServiceVendors => Set<ServiceVendor>();
        public DbSet<AgentTransaction> AgentTransactions => Set<AgentTransaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgentEconomyDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
