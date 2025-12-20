using AiAgentEconomy.AgentRuntime.AI.Kernel;
using AiAgentEconomy.AgentRuntime.AI.Tools;
using AiAgentEconomy.AgentRuntime.Messaging.Contracts;
using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.AgentRuntime.AI.Agents
{
    public sealed class TransactionAgent(
    KernelFactory kernelFactory,
    TransactionLifecycleTool tool,
    ILogger<TransactionAgent> logger)
    {
        public async Task RunAsync(TransactionApproved evt, CancellationToken ct)
        {
            var kernel = kernelFactory.CreateKernel();

            // Tool'u kernel'a "function calling" için ekliyoruz.
            // Not: Bu yaklaşım SK sürümlerinde değişebiliyor; ama temel mantık: native functions -> tools.
            kernel.Plugins.AddFromObject(tool, pluginName: "tx");

            var chat = kernel.GetRequiredService<IChatCompletionService>();

            var system = """
                            You are an autonomous transaction operator for AiAgentEconomy.
                            When a TransactionApproved event arrives, decide the safest next action.
                            Rules:
                            - If transaction is approved, submit it.
                            - After successful submit, settle it.
                            - If any step fails, return FAIL with a reason code.
                            Respond in a single line: SUBMIT_AND_SETTLE or FAIL:<REASON_CODE>
                            """;

            var user = $"TransactionApproved: txId={evt.TransactionId} agentId={evt.AgentId} amount={evt.Amount} currency={evt.Currency}";

            var history = new ChatHistory(system);
            history.AddUserMessage(user);

            // Model kararını al
            var response = await chat.GetChatMessageContentAsync(history, cancellationToken: ct);
            var decision = (response.Content ?? string.Empty).Trim();

            logger.LogInformation("AI decision for txId={TxId}: {Decision}", evt.TransactionId, decision);

            // MVP: iki karar tipi
            if (decision.StartsWith("FAIL:", StringComparison.OrdinalIgnoreCase))
            {
                var reason = decision.Substring("FAIL:".Length).Trim();
                if (string.IsNullOrWhiteSpace(reason)) reason = "AI_DECISION_FAIL";

                await tool.FailAsync(evt.TransactionId, reason, ct);
                return;
            }

            // Default: submit+settle
            await tool.SubmitAsync(evt.TransactionId, ct);
            await tool.SettleAsync(evt.TransactionId, ct);
        }
    }
}
