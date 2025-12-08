using AiAgentEconomy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Entities
{
    public class AgentTransaction
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public Guid WalletId { get; set; }
        public Guid ServiceVendorId { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string BlockchainTxHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public AgentTransactionStatus Status { get; set; }
    }
}
