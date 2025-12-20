using AiAgentEconomy.AgentRuntime.AI.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

namespace AiAgentEconomy.AgentRuntime.AI.Kernel
{
    public sealed class KernelFactory(IOptions<OpenAiOptions> openAiOptions)
    {
        public Kernel CreateKernel()
        {
            var opts = openAiOptions.Value;

            if (string.IsNullOrWhiteSpace(opts.ApiKey))
                throw new InvalidOperationException("OpenAI:ApiKey is missing. Set it via user-secrets or configuration.");

            var builder = Microsoft.SemanticKernel.Kernel.CreateBuilder();

            // OpenAI chat completion
            builder.Services.AddOpenAIChatCompletion(
                modelId: opts.Model,
                apiKey: opts.ApiKey);

            return builder.Build();
        }
    }
}
