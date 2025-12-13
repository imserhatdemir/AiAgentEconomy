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

        public bool CanSpend(decimal amount, DateTime utcNow, out string reason)
        {
            // Backward-compatible wrapper for older callers (e.g., Agent.CanSpend)
            return CanSpend(amount, utcNow, vendor: null, serviceCode: null, out reason);
        }
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
            reason = string.Empty;

            if (!IsActive)
            {
                reason = "POLICY_INACTIVE";
                return false;
            }

            // Per-transaction limit
            if (MaxPerTransaction > 0 && amount > MaxPerTransaction)
            {
                reason = "PER_TX_LIMIT_EXCEEDED";
                return false;
            }

            // Daily window tracking
            var today = DateOnly.FromDateTime(utcNow.Date);

            if (DailyWindowDate is null || DailyWindowDate.Value != today)
            {
                DailyWindowDate = today;
                SpentInDailyWindow = 0;
            }

            if (DailyLimit > 0 && SpentInDailyWindow + amount > DailyLimit)
            {
                reason = "DAILY_LIMIT_EXCEEDED";
                return false;
            }

            // Vendor allowlist
            vendor = string.IsNullOrWhiteSpace(vendor) ? null : vendor.Trim();
            serviceCode = string.IsNullOrWhiteSpace(serviceCode) ? null : serviceCode.Trim();

            if (!string.IsNullOrWhiteSpace(AllowedVendorsCsv))
            {
                if (vendor is null)
                {
                    reason = "VENDOR_REQUIRED";
                    return false;
                }

                var allowedVendors = AllowedVendorsCsv
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (!allowedVendors.Any(x => x.Equals(vendor, StringComparison.OrdinalIgnoreCase)))
                {
                    reason = "VENDOR_NOT_ALLOWED";
                    return false;
                }
            }

            // Service allowlist
            if (!string.IsNullOrWhiteSpace(AllowedServicesCsv))
            {
                if (serviceCode is null)
                {
                    reason = "SERVICE_REQUIRED";
                    return false;
                }

                var allowedServices = AllowedServicesCsv
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (!allowedServices.Any(x => x.Equals(serviceCode, StringComparison.OrdinalIgnoreCase)))
                {
                    reason = "SERVICE_NOT_ALLOWED";
                    return false;
                }
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
