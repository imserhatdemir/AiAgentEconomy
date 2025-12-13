using AiAgentEconomy.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Transactions
{
    public class Transaction : AuditableEntity
    {
        public Guid AgentId { get; private set; }
        public Guid WalletId { get; private set; }

        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = "USDC";

        public TransactionType Type { get; private set; }
        public TransactionStatus Status { get; private set; }

        public string? RejectionReason { get; private set; }
        public string? Vendor { get; private set; }
        public string? ServiceCode { get; private set; }
        private Transaction() { } // EF Core

        private Transaction(
            Guid agentId,
            Guid walletId,
            decimal amount,
            TransactionType type,
            string currency, string? vendor, string? serviceCode)
        {
            AgentId = agentId;
            WalletId = walletId;
            Amount = amount;
            Type = type;
            Currency = currency;
            Status = TransactionStatus.Pending;
            Vendor = vendor;
            ServiceCode = serviceCode;
        }

        public static Transaction Create(
            Guid agentId,
            Guid walletId,
            decimal amount,
            TransactionType type,
            string currency = "USDC",string? vendor = null, string? serviceCode = null)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Transaction amount must be greater than zero.");

            vendor = string.IsNullOrWhiteSpace(vendor) ? null : vendor.Trim();
            serviceCode = string.IsNullOrWhiteSpace(serviceCode) ? null : serviceCode.Trim();

            return new Transaction(agentId, walletId, amount, type, currency, vendor, serviceCode);
        }

        public void Approve()
        {
            Status = TransactionStatus.Approved;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void Reject(string reason)
        {
            Status = TransactionStatus.Rejected;
            RejectionReason = reason;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void MarkOnChainSent()
        {
            Status = TransactionStatus.OnChainSent;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void Confirm()
        {
            Status = TransactionStatus.Confirmed;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void Fail(string reason)
        {
            Status = TransactionStatus.Failed;
            RejectionReason = reason;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
