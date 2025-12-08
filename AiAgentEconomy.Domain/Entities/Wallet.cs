using AiAgentEconomy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Chain { get; set; }          // "Arbitrum One", "Sepolia Arbitrum" vs.
        public string Address { get; set; }
        public WalletType Type { get; set; }       // Custodial, NonCustodial, SmartContract
    }
}
