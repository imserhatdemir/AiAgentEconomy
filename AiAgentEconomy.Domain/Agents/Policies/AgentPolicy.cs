using AiAgentEconomy.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Agents.Policies
{
    public class AgentPolicy : AuditableEntity
    {
        // Ownership
        public Guid AgentId { get; set; }

        // Meta
        public string Name { get; set; } = "Default Policy";
        public bool IsActive { get; set; } = true;

        // Limits
        public decimal MaxPerTransaction { get; set; }      // e.g. 5 USDC
        public decimal DailyLimit { get; set; }             // e.g. 20 USDC

        // Currency
        public string Currency { get; set; } = "USDC";

        // Optional allowlists (CSV for MVP)
        public string? AllowedVendorsCsv { get; set; }      // "VendorA,VendorB"
        public string? AllowedServicesCsv { get; set; }     // "Service1,Service2"

        // Daily spend tracking (policy-level)
        public DateOnly? DailyWindowDate { get; set; }      // UTC day
        public decimal SpentInDailyWindow { get; set; }

        /// <summary>
        /// Determines whether the agent can spend the given amount
        /// according to policy rules.
        /// </summary>
        public bool CanSpend(
            decimal amount,
            DateTime utcNow,
            string? vendor,
            string? serviceCode,
            out string reason)
        {
            // First run existing checks (per-tx + daily)
            if (!CanSpend(amount, utcNow, null, null, out reason))
                return false;

            // Vendor allowlist (optional)
            if (!string.IsNullOrWhiteSpace(AllowedVendorsCsv) && !string.IsNullOrWhiteSpace(vendor))
            {
                var allowedVendors = AllowedVendorsCsv
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                var ok = allowedVendors.Any(x => x.Equals(vendor, StringComparison.OrdinalIgnoreCase));
                if (!ok)
                {
                    reason = "VENDOR_NOT_ALLOWED";
                    return false;
                }
            }
            else if (!string.IsNullOrWhiteSpace(AllowedVendorsCsv) && string.IsNullOrWhiteSpace(vendor))
            {
                // Policy vendor restricted but request missing vendor info
                reason = "VENDOR_REQUIRED";
                return false;
            }

            // Service allowlist (optional)
            if (!string.IsNullOrWhiteSpace(AllowedServicesCsv) && !string.IsNullOrWhiteSpace(serviceCode))
            {
                var allowedServices = AllowedServicesCsv
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                var ok = allowedServices.Any(x => x.Equals(serviceCode, StringComparison.OrdinalIgnoreCase));
                if (!ok)
                {
                    reason = "SERVICE_NOT_ALLOWED";
                    return false;
                }
            }
            else if (!string.IsNullOrWhiteSpace(AllowedServicesCsv) && string.IsNullOrWhiteSpace(serviceCode))
            {
                reason = "SERVICE_REQUIRED";
                return false;
            }

            reason = string.Empty;
            return true;
        }

        /// <summary>
        /// Updates daily spend after an approved transaction.
        /// </summary>
        public void AddDailySpend(decimal amount, DateTime utcNow)
        {
            if (amount <= 0) return;

            var today = DateOnly.FromDateTime(utcNow.Date);

            if (DailyWindowDate is null || DailyWindowDate.Value != today)
            {
                DailyWindowDate = today;
                SpentInDailyWindow = 0;
            }

            SpentInDailyWindow += amount;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
