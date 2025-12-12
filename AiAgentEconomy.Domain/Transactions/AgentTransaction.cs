using AiAgentEconomy.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Transactions
{
    public class AgentTransaction : AuditableEntity
    {
        public Guid AgentId { get; set; }
        public Guid WalletId { get; set; }
        public Guid ServiceVendorId { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USDC";

        public string? BlockchainTxHash { get; set; }

        public AgentTransactionStatus Status { get; set; } = AgentTransactionStatus.Pending;

        // Optional metadata for debugging/audit
        public string? MetadataJson { get; set; }
    }
}
