using AiAgentEconomy.Domain.Common;
using AiAgentEconomy.Domain.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Agents
{
    public class Agent : AuditableEntity
    {
        public Guid UserId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Goal { get; set; } = string.Empty;

        public AgentStatus Status { get; set; } = AgentStatus.Active;
        public AgentRiskLevel RiskLevel { get; set; } = AgentRiskLevel.Low;

        // Budget
        public decimal MonthlyBudget { get; set; }
        public decimal SpentThisMonth { get; set; }

        // Relationships
        public Guid? WalletId { get; set; }
        public Wallet? Wallet { get; set; }

        public Guid? PolicyId { get; set; }
        public AgentPolicy? Policy { get; set; }
        public string? Description { get; set; }

        public bool CanSpend(decimal amount, DateTime utcNow, out string reason)
        {
            reason = string.Empty;

            if (Status != AgentStatus.Active)
            {
                reason = "AGENT_NOT_ACTIVE";
                return false;
            }

            if (amount <= 0)
            {
                reason = "INVALID_AMOUNT";
                return false;
            }

            if (SpentThisMonth + amount > MonthlyBudget)
            {
                reason = "MONTHLY_BUDGET_EXCEEDED";
                return false;
            }

            if (Policy is null)
            {
                // Policy yoksa default: sadece monthly budget kontrolü
                return true;
            }

            return Policy.CanSpend(amount, utcNow, out reason);
        }

        public void AddSpent(decimal amount)
        {
            if (amount <= 0) return;
            SpentThisMonth += amount;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
