using AiAgentEconomy.Application.Exceptions;
using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Transactions;
using AiAgentEconomy.Domain.Agents;
using AiAgentEconomy.Domain.Transactions;
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

        public TransactionService(
            IAgentRepository agentRepo,
            IWalletRepository walletRepo,
            ITransactionRepository txRepo,
            IAgentPolicyRepository policyRepo)
        {
            _agentRepo = agentRepo;
            _walletRepo = walletRepo;
            _txRepo = txRepo;
            _policyRepo = policyRepo;
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

            // 4) Create tx (Pending -> will become Approved/Rejected)
            var tx = Transaction.Create(
                                            agentId,
                                            wallet.Id,
                                            request.Amount,
                                            txType,
                                            currency,
                                            request.Vendor,
                                            request.ServiceCode
                                        );

            // 5) Decision pipeline (Agent status + monthly budget + policy)
            string? rejectReason = null;

            if (agent.Status != AgentStatus.Active)
            {
                rejectReason = "AGENT_NOT_ACTIVE";
            }
            else if (agent.SpentThisMonth + request.Amount > agent.MonthlyBudget)
            {
                rejectReason = "MONTHLY_BUDGET_EXCEEDED";
            }
            else
            {
                // Policy (tracked)
                var policy = await _policyRepo.GetByAgentIdForUpdateAsync(agentId, ct);

                if (policy is not null)
                {
                    // Optional currency mismatch gate (recommended)
                    if (!string.Equals(policy.Currency, currency, StringComparison.OrdinalIgnoreCase))
                    {
                        rejectReason = "POLICY_CURRENCY_MISMATCH";
                    }
                    else if (!policy.CanSpend(request.Amount, now, request.Vendor, request.ServiceCode, out var policyReason))
                    {
                        rejectReason = policyReason;
                    }
                    else
                    {
                        // Approved: reserve budgets
                        tx.Approve();
                        agent.AddSpent(request.Amount);
                        policy.AddDailySpend(request.Amount, now);
                    }
                }
                else
                {
                    // No policy => monthly budget only
                    tx.Approve();
                    agent.AddSpent(request.Amount);
                }
            }

            if (!string.IsNullOrWhiteSpace(rejectReason))
                tx.Reject(rejectReason);

            // 6) Persist
            await _txRepo.AddAsync(tx, ct);
            await _txRepo.SaveChangesAsync(ct);

            return ToDto(tx);
        }

        public async Task<IReadOnlyList<TransactionDto>> GetByAgentAsync(Guid agentId, int take = 50, CancellationToken ct = default)
        {
            var list = await _txRepo.GetByAgentIdAsync(agentId, take, ct);
            return list.Select(ToDto).ToList();
        }

        private static TransactionDto ToDto(Domain.Transactions.Transaction tx)
            => new(
                tx.Id,
                tx.AgentId,
                tx.WalletId,
                tx.Amount,
                tx.Currency,
                tx.Type.ToString(),
                tx.Status.ToString(),
                tx.RejectionReason,
                tx.Vendor,
                tx.ServiceCode,
                tx.CreatedAtUtc
            );
    }
}
