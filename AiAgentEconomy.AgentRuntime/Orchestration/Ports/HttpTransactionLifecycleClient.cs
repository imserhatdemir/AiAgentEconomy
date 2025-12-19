using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.AgentRuntime.Orchestration.Ports
{
    public sealed class HttpTransactionLifecycleClient(
        HttpClient http,
        IConfiguration config,
        ILogger<HttpTransactionLifecycleClient> logger
    ) : ITransactionLifecycleClient
    {
        private string BasePath =>
            config["AgentRuntime:TransactionsPath"]?.TrimEnd('/')
            ?? throw new InvalidOperationException("AgentRuntime:TransactionsPath is not configured.");

        public async Task SubmitAsync(Guid transactionId, SubmitTransactionHttpRequest request, CancellationToken ct = default)
        {
            var url = $"{BasePath}/{transactionId}/submit";
            logger.LogInformation("HTTP: POST {Url}", url);

            using var resp = await http.PostAsJsonAsync(url, request, ct);
            await EnsureSuccess(resp, ct, "submit");
        }

        public async Task SettleAsync(Guid transactionId, CancellationToken ct = default)
        {
            var url = $"{BasePath}/{transactionId}/settle";
            logger.LogInformation("HTTP: POST {Url}", url);

            using var resp = await http.PostAsync(url, content: null, ct);
            await EnsureSuccess(resp, ct, "settle");
        }

        public async Task FailAsync(Guid transactionId, string reason, CancellationToken ct = default)
        {
            var url = $"{BasePath}/{transactionId}/fail";
            logger.LogInformation("HTTP: POST {Url}", url);

            using var resp = await http.PostAsJsonAsync(url, new { reason }, ct);
            await EnsureSuccess(resp, ct, "fail");
        }

        private static async Task EnsureSuccess(HttpResponseMessage resp, CancellationToken ct, string op)
        {
            if (resp.IsSuccessStatusCode) return;

            var body = await SafeReadBody(resp, ct);
            throw new Exception($"HTTP {op} failed. Status={(int)resp.StatusCode} Body={body}");
        }

        private static async Task<string> SafeReadBody(HttpResponseMessage resp, CancellationToken ct)
        {
            try { return await resp.Content.ReadAsStringAsync(ct); }
            catch { return "<unreadable>"; }
        }
    }
}
