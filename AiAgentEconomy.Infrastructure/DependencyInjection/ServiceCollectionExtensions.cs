using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Application.Services;
using AiAgentEconomy.Infrastructure.Persistence;
using AiAgentEconomy.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AiAgentEconomy.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connStr = configuration.GetConnectionString("Postgres");
            services.AddDbContext<AgentEconomyDbContext>(opt =>
                opt.UseNpgsql(connStr));
            services.AddScoped<IAgentRepository, AgentRepository>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IWalletService, WalletService>();
            return services;
        }
    }
}
