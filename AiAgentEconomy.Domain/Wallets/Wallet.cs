using AiAgentEconomy.Domain.Agents;
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
        public Guid AgentId { get; private set; }

        public string Chain { get; set; } = "Arbitrum";
        public string Address { get; set; } = string.Empty;

        public WalletType Type { get; set; } = WalletType.NonCustodial;

        public bool IsActive { get; set; } = true;

        private Wallet() { }
        public Wallet(
        Guid agentId,
        string chain,
        string address,
        WalletType type)
        {
            AgentId = agentId;
            Chain = chain;
            Address = address;
            Type = type;
            IsActive = true;
        }
        public void Deactivate()
        {
            IsActive = false;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }

}
