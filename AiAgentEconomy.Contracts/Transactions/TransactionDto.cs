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
        DateTime CreatedAtUtc,

        // Marketplace snapshot
        Guid? VendorId,
        Guid? MarketplaceServiceId,
        decimal? UnitPrice,
        string? UnitPriceCurrency,

        // On-chain metadata
        string? Chain,
        string? Network,
        string? BlockchainTxHash,
        string? ExplorerUrl,
        DateTime? SubmittedAtUtc,
        DateTime? SettledAtUtc,
        DateTime? FailedAtUtc,
        string? FailureReason
    );
}
