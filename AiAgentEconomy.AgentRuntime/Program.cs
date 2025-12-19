using AiAgentEconomy.AgentRuntime.Hosting;
using Microsoft.Extensions.Hosting;

namespace AiAgentEconomy.AgentRuntime;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Core AgentRuntime registrations (Policies, Observability, Messaging abstractions, etc.)
        builder.Services.AddAgentRuntime(builder.Configuration);

        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();
        host.Run();
    }
}
