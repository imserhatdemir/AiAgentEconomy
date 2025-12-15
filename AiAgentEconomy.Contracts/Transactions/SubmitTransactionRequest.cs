using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Contracts.Transactions
{
    public sealed class SubmitTransactionRequest
    {
        public string Chain { get; set; } = "Arbitrum";
        public string Network { get; set; } = "arbitrum-sepolia";

        // Optional: Client can pass it; otherwise we will compute in service later if needed
        public string? ExplorerUrl { get; set; }
    }
}
