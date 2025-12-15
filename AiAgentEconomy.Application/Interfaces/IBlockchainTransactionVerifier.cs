using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface IBlockchainTransactionVerifier
    {
        /// <summary>
        /// Checks the on-chain status for a given transaction hash.
        /// </summary>
        Task<BlockchainVerifyResult> VerifyAsync(string chain, string network, string txHash, CancellationToken ct = default);
    }

    public sealed record BlockchainVerifyResult(
        bool IsConfirmed,
        bool IsSuccess,
        string? FailureReason
    );
}
