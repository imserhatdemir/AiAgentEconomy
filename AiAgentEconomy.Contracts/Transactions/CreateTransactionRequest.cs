using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Contracts.Transactions
{
    public sealed record CreateTransactionRequest(
        decimal Amount,
        string Type,
        string? Currency,
        string? Vendor,
        string? ServiceCode
    );
}
