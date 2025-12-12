using AiAgentEconomy.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Wallets
{
    public class Wallet : AuditableEntity
    {
        public Guid UserId { get; set; }

        public string Chain { get; set; } = "Arbitrum";
        public string Address { get; set; } = string.Empty;

        public WalletType Type { get; set; } = WalletType.NonCustodial;

        public bool IsActive { get; set; } = true;
    }

}
