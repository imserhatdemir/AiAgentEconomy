using AiAgentEconomy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Entities
{
    public class Agent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }          // "API Hunter", "Subscription Manager" vb.
        public string Goal { get; set; }          // Doğal dil: "X servisini en iyi fiyata satın al"
        public decimal MonthlyBudget { get; set; }
        public decimal SpentThisMonth { get; set; }
        public AgentRiskLevel RiskLevel { get; set; } // Low, Medium, High
        public AgentStatus Status { get; set; }       // Active, Paused, Suspended

        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public ICollection<AgentPolicy> Policies { get; set; } = new List<AgentPolicy>();
    }
}
