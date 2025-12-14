using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface IBlockchainTransactionSender
    {
        /// <summary>
        /// Submits a transaction to the blockchain network and returns the transaction hash.
        /// </summary>
        Task<BlockchainSubmitResult> SubmitAsync(BlockchainSubmitCommand command, CancellationToken ct = default);
    }

    public sealed record BlockchainSubmitCommand(
        string Chain,
        string Network,
        string FromAddress,
        string ToAddress,
        decimal Amount,
        string Currency,
        string? Vendor,
        string? ServiceCode
    );

    public sealed record BlockchainSubmitResult(
        string TxHash,
        string? ExplorerUrl
    );
}
