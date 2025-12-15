using AiAgentEconomy.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Infrastructure.Blockchain
{
    public sealed class FakeBlockchainTransactionVerifier : IBlockchainTransactionVerifier
    {
        public Task<BlockchainVerifyResult> VerifyAsync(string chain, string network, string txHash, CancellationToken ct = default)
        {
            // MVP behavior: confirmed + success always
            return Task.FromResult(new BlockchainVerifyResult(
                IsConfirmed: true,
                IsSuccess: true,
                FailureReason: null
            ));
        }
    }
}
