using AiAgentEconomy.Domain.Common;

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
        public Guid? VendorId { get; private set; }
        public Guid? MarketplaceServiceId { get; private set; }
        public decimal? UnitPrice { get; private set; }   // marketplace price snapshot
        public string? UnitPriceCurrency { get; private set; } // snapshot currency (optional but recommended)
        public string? Chain { get; private set; }              // e.g. "Arbitrum"
        public string? Network { get; private set; }            // e.g. "sepolia", "arbitrum-sepolia"
        public string? BlockchainTxHash { get; private set; }   // 0x...
        public string? ExplorerUrl { get; private set; }        // computed or stored snapshot

        public DateTime? SubmittedAtUtc { get; private set; }
        public DateTime? SettledAtUtc { get; private set; }
        public DateTime? FailedAtUtc { get; private set; }
        public string? FailureReason { get; private set; }
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
            if (Status != TransactionStatus.Pending)
                throw new InvalidOperationException("Only Pending transactions can be approved.");

            Status = TransactionStatus.Approved;
            UpdatedAtUtc = DateTime.UtcNow;
        }
        public void Reject(string reason)
        {
            if (Status != TransactionStatus.Pending)
                throw new InvalidOperationException("Only Pending transactions can be rejected.");

            Status = TransactionStatus.Rejected;
            RejectionReason = string.IsNullOrWhiteSpace(reason) ? "REJECTED" : reason.Trim();
            UpdatedAtUtc = DateTime.UtcNow;
        }
        public void Fail(string reason)
        {
            // obsolete: use MarkFailed
            MarkFailed(reason);
        }
        public void AttachMarketplace(Guid vendorId, Guid marketplaceServiceId, decimal unitPrice, string currency)
        {
            VendorId = vendorId;
            MarketplaceServiceId = marketplaceServiceId;
            UnitPrice = unitPrice;
            UnitPriceCurrency = currency;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void MarkSubmitted(string chain, string network, string txHash, string? explorerUrl = null)
        {
            if (Status != TransactionStatus.Approved)
                throw new InvalidOperationException("Only Approved transactions can be submitted on-chain.");

            if (string.IsNullOrWhiteSpace(txHash))
                throw new ArgumentException("txHash is required.", nameof(txHash));

            Chain = chain;
            Network = network;
            BlockchainTxHash = txHash.Trim();
            ExplorerUrl = explorerUrl;

            SubmittedAtUtc = DateTime.UtcNow;
            Status = TransactionStatus.Submitted;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void MarkSettled()
        {
            if (Status != TransactionStatus.Submitted)
                throw new InvalidOperationException("Only Submitted transactions can be settled.");

            SettledAtUtc = DateTime.UtcNow;
            Status = TransactionStatus.Settled;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void MarkFailed(string reason)
        {
            if (Status != TransactionStatus.Submitted)
                throw new InvalidOperationException("Only Submitted transactions can be failed.");

            FailureReason = string.IsNullOrWhiteSpace(reason) ? "ONCHAIN_FAILED" : reason.Trim();
            FailedAtUtc = DateTime.UtcNow;
            Status = TransactionStatus.Failed;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
