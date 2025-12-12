using AiAgentEconomy.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Agents
{
    public class AgentPolicy : AuditableEntity
    {
        public Guid AgentId { get; set; }

        // Limits
        public decimal MaxPerTransaction { get; set; }          // örn: 5 USDC
        public decimal DailyLimit { get; set; }                 // örn: 20 USDC

        // Currency
        public string Currency { get; set; } = "USDC";

        // Optional vendor/service constraints (MVP’de string listeler yeterli)
        public string? AllowedVendorsCsv { get; set; }          // "VendorA,VendorB"
        public string? AllowedServicesCsv { get; set; }         // "Service1,Service2"

        // Daily spend tracking (MVP için domain içinde basit alanlar; ileride Tx’lerden hesaplatırız)
        public DateOnly? DailyWindowDate { get; set; }          // UTC date
        public decimal SpentInDailyWindow { get; set; }

        public bool CanSpend(decimal amount, DateTime utcNow, out string reason)
        {
            reason = string.Empty;

            if (MaxPerTransaction > 0 && amount > MaxPerTransaction)
            {
                reason = "PER_TX_LIMIT_EXCEEDED";
                return false;
            }

            var today = DateOnly.FromDateTime(utcNow.Date);

            if (DailyWindowDate is null || DailyWindowDate.Value != today)
            {
                // Yeni gün başladığında sıfırla
                DailyWindowDate = today;
                SpentInDailyWindow = 0;
            }

            if (DailyLimit > 0 && (SpentInDailyWindow + amount) > DailyLimit)
            {
                reason = "DAILY_LIMIT_EXCEEDED";
                return false;
            }

            return true;
        }

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
