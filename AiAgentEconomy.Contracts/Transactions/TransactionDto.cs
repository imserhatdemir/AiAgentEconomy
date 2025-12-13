using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Contracts.Transactions
{
    public sealed record TransactionDto(
        Guid Id,
        Guid AgentId,
        Guid WalletId,
        decimal Amount,
        string Currency,
        string Type,
        string Status,
        string? RejectionReason,
        string? Vendor,
        string? ServiceCode,
        DateTime CreatedAtUtc
    );
}
