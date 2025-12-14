using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Contracts.Transactions
{
    public sealed class FailTransactionRequest
    {
        public string? Reason { get; set; } // optional, default ONCHAIN_FAILED
    }
}
