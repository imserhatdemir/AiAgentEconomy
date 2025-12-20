using AiAgentEconomy.Application.Exceptions;
using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Transactions;
using AiAgentEconomy.Domain.Agents;
using AiAgentEconomy.Domain.Transactions;
using AiAgentEconomy.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Application.Services
{
    public sealed class TransactionService : ITransactionService
    {
        private readonly IAgentRepository _agentRepo;
        private readonly IWalletRepository _walletRepo;
        private readonly ITransactionRepository _txRepo;
        private readonly IAgentPolicyRepository _policyRepo;
        private readonly IMarketplaceRepository _marketplaceRepo;
        private readonly IBlockchainTransactionSender _blockchainSender;
        private readonly IEventPublisher _publisher;

        public TransactionService(
            IAgentRepository agentRepo,
            IWalletRepository walletRepo,
            ITransactionRepository txRepo,
            IAgentPolicyRepository policyRepo,
            IMarketplaceRepository marketplaceRepo,
            IBlockchainTransactionSender blockchainSender,
            IEventPublisher publisher)
        {
            _agentRepo = agentRepo;
            _walletRepo = walletRepo;
            _txRepo = txRepo;
            _policyRepo = policyRepo;
            _marketplaceRepo = marketplaceRepo;
            _blockchainSender = blockchainSender;
            _publisher = publisher;
        }

        public async Task<TransactionDto> CreateForAgentAsync(
    Guid agentId,
    CreateTransactionRequest request,
    CancellationToken ct = default)
        {
            // 1) Agent (tracked)
            var agent = await _agentRepo.GetByIdForUpdateAsync(agentId, ct);
            if (agent is null)
                throw new NotFoundException("Agent not found.");

            // 2) Wallet (Agent -> Wallet 1-1)
            var wallet = await _walletRepo.GetByAgentIdAsync(agentId, ct);
            if (wallet is null)
                throw new NotFoundException("Wallet not found for this agent.");

            if (!wallet.IsActive)
                throw new ConflictException("Wallet is inactive.");

            // 3) Validate request
            if (request.Amount <= 0)
                throw new ValidationException("Amount must be greater than zero.");

            if (!Enum.TryParse<TransactionType>(request.Type, ignoreCase: true, out var txType))
                throw new ValidationException("Invalid transaction type.");

            var currency = string.IsNullOrWhiteSpace(request.Currency) ? "USDC" : request.Currency.Trim();
            var now = DateTime.UtcNow;

            var vendor = string.IsNullOrWhiteSpace(request.Vendor) ? null : request.Vendor.Trim();
            var serviceCode = string.IsNullOrWhiteSpace(request.ServiceCode) ? null : request.ServiceCode.Trim();

            // 4) Create tx (Pending -> Approved/Rejected)
            var tx = Transaction.Create(
                agentId,
                wallet.Id,
                request.Amount,
                txType,
                currency,
                vendor,
                serviceCode
            );

            // Snapshot variables (to attach to tx if validated)
            Guid? vendorId = null;
            Guid? marketplaceServiceId = null;
            decimal? unitPrice = null;
            string? unitPriceCurrency = null;

            // Decision pipeline
            string? rejectReason = null;
            var approved = false;

            // 5.1 Agent gates
            if (agent.Status != AgentStatus.Active)
            {
                rejectReason = "AGENT_NOT_ACTIVE";
            }
            else if (agent.SpentThisMonth + request.Amount > agent.MonthlyBudget)
            {
                rejectReason = "MONTHLY_BUDGET_EXCEEDED";
            }

            // 5.2 Marketplace gates (MVP: ServicePurchase must match marketplace)
            if (rejectReason is null && txType == TransactionType.ServicePurchase)
            {
                if (string.IsNullOrWhiteSpace(vendor))
                {
                    rejectReason = "VENDOR_REQUIRED";
                }
                else if (string.IsNullOrWhiteSpace(serviceCode))
                {
                    rejectReason = "SERVICE_REQUIRED";
                }
                else
                {
                    var vendorEntity = await _marketplaceRepo.GetVendorByNameAsync(vendor, ct);
                    if (vendorEntity is null)
                    {
                        rejectReason = "MARKETPLACE_VENDOR_NOT_FOUND";
                    }
                    else if (!vendorEntity.IsActive)
                    {
                        rejectReason = "MARKETPLACE_VENDOR_INACTIVE";
                    }
                    else
                    {
                        var svc = await _marketplaceRepo.GetServiceAsync(vendorEntity.Id, serviceCode, ct);
                        if (svc is null)
                        {
                            rejectReason = "MARKETPLACE_SERVICE_NOT_FOUND";
                        }
                        else if (!svc.IsActive)
                        {
                            rejectReason = "MARKETPLACE_SERVICE_INACTIVE";
                        }
                        else if (!string.Equals(svc.Currency, currency, StringComparison.OrdinalIgnoreCase))
                        {
                            rejectReason = "MARKETPLACE_CURRENCY_MISMATCH";
                        }
                        else if (svc.Price != request.Amount)
                        {
                            rejectReason = "MARKETPLACE_PRICE_MISMATCH";
                        }
                        else
                        {
                            // Marketplace validated -> prepare snapshot
                            vendorId = vendorEntity.Id;
                            marketplaceServiceId = svc.Id;
                            unitPrice = svc.Price;
                            unitPriceCurrency = svc.Currency;
                        }
                    }
                }
            }

            // 5.3 Policy gates (after marketplace validation)
            if (rejectReason is null)
            {
                var policy = await _policyRepo.GetByAgentIdForUpdateAsync(agentId, ct);

                if (policy is not null)
                {
                    if (!policy.IsActive)
                    {
                        rejectReason = "POLICY_INACTIVE";
                    }
                    else if (!string.Equals(policy.Currency, currency, StringComparison.OrdinalIgnoreCase))
                    {
                        rejectReason = "POLICY_CURRENCY_MISMATCH";
                    }
                    else if (!policy.CanSpend(request.Amount, now, vendor, serviceCode, out var policyReason))
                    {
                        rejectReason = policyReason;
                    }
                    else
                    {
                        // Approved: reserve budgets
                        tx.Approve();
                        approved = true;

                        agent.AddSpent(request.Amount);
                        policy.AddDailySpend(request.Amount, now);
                    }
                }
                else
                {
                    // No policy => monthly budget only (already checked)
                    tx.Approve();
                    approved = true;

                    agent.AddSpent(request.Amount);
                }
            }

            // 5.4 Finalize tx
            if (!string.IsNullOrWhiteSpace(rejectReason))
            {
                tx.Reject(rejectReason);
            }
            else
            {
                // Attach marketplace snapshot if validated
                if (vendorId.HasValue && marketplaceServiceId.HasValue && unitPrice.HasValue)
                {
                    tx.AttachMarketplace(
                        vendorId.Value,
                        marketplaceServiceId.Value,
                        unitPrice.Value,
                        unitPriceCurrency ?? currency
                    );
                }
            }

            // 6) Persist
            await _txRepo.AddAsync(tx, ct);
            await _txRepo.SaveChangesAsync(ct);

            // 7) Publish (only if Approved)
            if (approved)
            {
                var evt = new TransactionApprovedEvent(
                    TransactionId: tx.Id,
                    AgentId: tx.AgentId,
                    Amount: tx.Amount,
                    Currency: tx.Currency,
                    CorrelationId: Guid.NewGuid().ToString("N"),
                    OccurredAt: DateTimeOffset.UtcNow
                );

                await _publisher.PublishAsync(evt, routingKey: "transactions.approved", ct);
            }

            return ToDto(tx);
        }

        public async Task<IReadOnlyList<TransactionDto>> GetByAgentAsync(Guid agentId, int take = 50, CancellationToken ct = default)
        {
            var list = await _txRepo.GetByAgentIdAsync(agentId, take, ct);
            return list.Select(ToDto).ToList();
        }

        private static TransactionDto ToDto(Domain.Transactions.Transaction tx)
        => new(
            // Identity
            tx.Id,
            tx.AgentId,
            tx.WalletId,

            // Core
            tx.Amount,
            tx.Currency,
            tx.Type.ToString(),
            tx.Status.ToString(),
            tx.RejectionReason,

            // Marketplace (request-level)
            tx.Vendor,
            tx.ServiceCode,

            // Marketplace snapshot (resolved)
            tx.VendorId,
            tx.MarketplaceServiceId,
            tx.UnitPrice,
            tx.UnitPriceCurrency,

            // On-chain metadata
            tx.Chain,
            tx.Network,
            tx.BlockchainTxHash,
            tx.ExplorerUrl,

            // Timestamps
            tx.CreatedAtUtc,
            tx.SubmittedAtUtc,
            tx.SettledAtUtc,
            tx.FailedAtUtc,
            tx.FailureReason
        );


        public async Task<TransactionDto> SubmitAsync(
                                            Guid transactionId,
                                            SubmitTransactionRequest request,
                                            CancellationToken ct = default)
        {
            var tx = await _txRepo.GetByIdForUpdateAsync(transactionId, ct);
            if (tx is null)
                throw new NotFoundException("Transaction not found.");

            // 1) Status guard (application-level → 409)
            if (tx.Status != TransactionStatus.Approved)
                throw new ConflictException("Only Approved transactions can be submitted on-chain.");

            // 2) Wallet
            var wallet = await _walletRepo.GetByIdAsync(tx.WalletId, ct);
            if (wallet is null)
                throw new NotFoundException("Wallet not found.");

            if (!wallet.IsActive)
                throw new ConflictException("Wallet is inactive.");

            // 3) Normalize chain / network
            var chain = string.IsNullOrWhiteSpace(request.Chain)
                ? "Arbitrum"
                : request.Chain.Trim();

            var network = string.IsNullOrWhiteSpace(request.Network)
                ? "arbitrum-sepolia"
                : request.Network.Trim();

            // 4) Submit to blockchain (FAKE provider for now)
            var submitResult = await _blockchainSender.SubmitAsync(
                new BlockchainSubmitCommand(
                    Chain: chain,
                    Network: network,
                    FromAddress: wallet.Address,
                    ToAddress: tx.Vendor ?? "MARKETPLACE",
                    Amount: tx.Amount,
                    Currency: tx.Currency,
                    Vendor: tx.Vendor,
                    ServiceCode: tx.ServiceCode
                ),
                ct
            );

            // 5) Domain transition
            tx.MarkSubmitted(
                chain: chain,
                network: network,
                txHash: submitResult.TxHash,
                explorerUrl: submitResult.ExplorerUrl ?? request.ExplorerUrl
            );

            await _txRepo.SaveChangesAsync(ct);
            return ToDto(tx);
        }



        public async Task<TransactionDto> SettleAsync(Guid transactionId, CancellationToken ct = default)
        {
            var tx = await _txRepo.GetByIdForUpdateAsync(transactionId, ct);
            if (tx is null)
                throw new NotFoundException("Transaction not found.");

            // Domain guard: only Submitted -> Settled
            tx.MarkSettled();

            await _txRepo.SaveChangesAsync(ct);
            return ToDto(tx);
        }

        public async Task<TransactionDto> FailAsync(Guid transactionId, FailTransactionRequest request, CancellationToken ct = default)
        {
            var tx = await _txRepo.GetByIdForUpdateAsync(transactionId, ct);
            if (tx is null)
                throw new NotFoundException("Transaction not found.");

            tx.MarkFailed(request.Reason);

            await _txRepo.SaveChangesAsync(ct);
            return ToDto(tx);
        }
    }
}
