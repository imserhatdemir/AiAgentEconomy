using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.AgentRuntime.Hosting
{
    public static class AppHost
    {
        public static Task RunAsync(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddAgentRuntime(builder.Configuration);

            var app = builder.Build();
            return app.RunAsync();
        }
    }
}
