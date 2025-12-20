using AiAgentEconomy.AgentRuntime.AI.Agents;
using AiAgentEconomy.AgentRuntime.AI.Configuration;
using AiAgentEconomy.AgentRuntime.AI.Kernel;
using AiAgentEconomy.AgentRuntime.AI.Tools;
using AiAgentEconomy.AgentRuntime.Memory;
using AiAgentEconomy.AgentRuntime.Messaging.Abstractions;
using AiAgentEconomy.AgentRuntime.Messaging.Consumers;
using AiAgentEconomy.AgentRuntime.Messaging.InMemory;
using AiAgentEconomy.AgentRuntime.Observability;
using AiAgentEconomy.AgentRuntime.Orchestration.Ports;
using AiAgentEconomy.AgentRuntime.Policies;       

namespace AiAgentEconomy.AgentRuntime.Hosting
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAgentRuntime(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IMessageBus, InMemoryMessageBus>();
            services.AddHostedService<TransactionApprovedConsumerHostedService>();

            services.AddSingleton<IAuditWriter, AuditWriter>();
            services.AddSingleton<IPolicyEvaluator, DefaultPolicyEvaluator>();

            services.AddSingleton<IProcessedEventStore, InMemoryProcessedEventStore>();

            services.AddHttpClient<ITransactionLifecycleClient, HttpTransactionLifecycleClient>(client =>
            {
                var baseUrl = config["AgentRuntime:ApiBaseUrl"];
                if (string.IsNullOrWhiteSpace(baseUrl))
                    throw new InvalidOperationException("AgentRuntime:ApiBaseUrl is not configured.");

                client.BaseAddress = new Uri(baseUrl);
            });
            services.Configure<OpenAiOptions>(
            config.GetSection(OpenAiOptions.SectionName));

            services.AddSingleton<KernelFactory>();
            services.AddSingleton<TransactionLifecycleTool>();
            services.AddSingleton<TransactionAgent>();
            return services;
        }
    }
}
