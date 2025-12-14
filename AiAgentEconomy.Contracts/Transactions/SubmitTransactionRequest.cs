using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Contracts.Transactions
{
    public sealed record SubmitTransactionRequest(
        string Chain,
        string Network,
        string TxHash,
        string? ExplorerUrl
    );
}
