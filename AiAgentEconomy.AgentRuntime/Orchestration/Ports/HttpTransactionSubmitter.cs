using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.AgentRuntime.Orchestration.Ports
{
    public sealed class HttpTransactionSubmitter(
        HttpClient http,
        IConfiguration config,
        ILogger<HttpTransactionSubmitter> logger
    ) : ITransactionSubmitter
    {
        public async Task SubmitAsync(Guid transactionId, CancellationToken ct = default)
        {
            var chain = "Arbitrum";
            var network = "arbitrum-sepolia";

            var req = new SubmitTransactionHttpRequest(
                Chain: chain,
                Network: network,
                ExplorerUrl: null
            );

            // Endpoint varsayımı: POST /api/transactions/{id}/submit
            // (Senin API route’un farklıysa burayı değiştireceğiz.)
            var url = $"/api/transactions/{transactionId}/submit";

            logger.LogInformation("HTTP-SUBMIT: POST {Url} txId={TxId}", url, transactionId);

            using var resp = await http.PostAsJsonAsync(url, req, ct);

            if (!resp.IsSuccessStatusCode)
            {
                var body = await SafeReadBody(resp, ct);
                throw new Exception($"HTTP submit failed. Status={(int)resp.StatusCode} Body={body}");
            }

            logger.LogInformation("HTTP-SUBMIT: Success txId={TxId}", transactionId);
        }

        private static async Task<string> SafeReadBody(HttpResponseMessage resp, CancellationToken ct)
        {
            try { return await resp.Content.ReadAsStringAsync(ct); }
            catch { return "<unreadable>"; }
        }
    }
}
