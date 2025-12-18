using AiAgentEconomy.AgentRuntime.Memory;
using AiAgentEconomy.AgentRuntime.Messaging.Abstractions;
using AiAgentEconomy.AgentRuntime.Messaging.Consumers;
using AiAgentEconomy.AgentRuntime.Messaging.InMemory;
using AiAgentEconomy.AgentRuntime.Observability;
using AiAgentEconomy.AgentRuntime.Orchestration.Ports;
using AiAgentEconomy.AgentRuntime.Policies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;               

namespace AiAgentEconomy.AgentRuntime.Hosting
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAgentRuntime(this IServiceCollection services, IConfiguration config)
        {
            // Observability
            services.AddSingleton<IAuditWriter, AuditWriter>();
            // Policies
            services.AddSingleton<IPolicyEvaluator, DefaultPolicyEvaluator>();
            services.AddSingleton<IMessageBus, InMemoryMessageBus>();
            services.AddHostedService<TransactionApprovedConsumerHostedService>();
            services.AddSingleton<IProcessedEventStore, InMemoryProcessedEventStore>();
            //services.AddSingleton<ITransactionSubmitter, FakeTransactionSubmitter>();
            services.AddHttpClient<ITransactionSubmitter, HttpTransactionSubmitter>(client =>
            {
                var baseUrl = config["AgentRuntime:ApiBaseUrl"];
                if (string.IsNullOrWhiteSpace(baseUrl))
                    throw new InvalidOperationException("AgentRuntime:ApiBaseUrl is not configured.");

                client.BaseAddress = new Uri(baseUrl);
            });
            services.AddHttpClient<ITransactionLifecycleClient, HttpTransactionLifecycleClient>(client =>
            {
                var baseUrl = config["AgentRuntime:ApiBaseUrl"];
                if (string.IsNullOrWhiteSpace(baseUrl))
                    throw new InvalidOperationException("AgentRuntime:ApiBaseUrl is not configured.");

                client.BaseAddress = new Uri(baseUrl);
            });

            return services;
        }
    }
}
