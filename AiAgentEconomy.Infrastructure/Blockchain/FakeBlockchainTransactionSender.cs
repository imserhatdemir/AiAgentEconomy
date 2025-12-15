using AiAgentEconomy.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Infrastructure.Blockchain
{
    public sealed class FakeBlockchainTransactionSender : IBlockchainTransactionSender
    {
        public Task<BlockchainSubmitResult> SubmitAsync(BlockchainSubmitCommand command, CancellationToken ct = default)
        {
            var txHash = "0x" + Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
            var explorer = $"https://explorer.fake/{command.Chain}/{command.Network}/tx/{txHash}";
            return Task.FromResult(new BlockchainSubmitResult(txHash, explorer));
        }
    }
}
