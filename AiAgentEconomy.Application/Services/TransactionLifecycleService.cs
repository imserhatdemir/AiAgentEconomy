using AiAgentEconomy.Application.Exceptions;
using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Transactions;
using AiAgentEconomy.Domain.Transactions;
namespace AiAgentEconomy.Application.Services
{
    public sealed class TransactionLifecycleService : ITransactionLifecycleService
    {
        private readonly ITransactionRepository _txRepo;
        private readonly IWalletRepository _walletRepo;
        private readonly IBlockchainTransactionSender _sender;
        private readonly IBlockchainTransactionVerifier _verifier;

        public TransactionLifecycleService(
            ITransactionRepository txRepo,
            IWalletRepository walletRepo,
            IBlockchainTransactionSender sender,
            IBlockchainTransactionVerifier verifier)
        {
            _txRepo = txRepo;
            _walletRepo = walletRepo;
            _sender = sender;
            _verifier = verifier;
        }

        public async Task<TransactionDto> SubmitAsync(Guid transactionId, SubmitTransactionRequest request, CancellationToken ct = default)
        {
            // tracked tx
            var tx = await _txRepo.GetByIdForUpdateAsync(transactionId, ct);
            if (tx is null)
                throw new NotFoundException("Transaction not found.");

            if (tx.Status != TransactionStatus.Approved)
                throw new ConflictException("Only Approved transactions can be submitted on-chain.");

            var wallet = await _walletRepo.GetByIdAsync(tx.WalletId, ct);
            if (wallet is null)
                throw new NotFoundException("Wallet not found for this transaction.");

            if (!wallet.IsActive)
                throw new ConflictException("Wallet is inactive.");

            var chain = string.IsNullOrWhiteSpace(request.Chain) ? "Arbitrum" : request.Chain.Trim();
            var network = string.IsNullOrWhiteSpace(request.Network) ? "arbitrum-sepolia" : request.Network.Trim();

            // MVP: toAddress vendor wallet address is not yet guaranteed on tx.
            // If we attached marketplace snapshot, you can set ToAddress = vendor wallet.
            // For now: use placeholder and focus on orchestration.
            var toAddress = "0x0000000000000000000000000000000000000000";

            var submit = await _sender.SubmitAsync(
                new BlockchainSubmitCommand(
                    Chain: chain,
                    Network: network,
                    FromAddress: wallet.Address,
                    ToAddress: toAddress,
                    Amount: tx.Amount,
                    Currency: tx.Currency,
                    Vendor: tx.Vendor,
                    ServiceCode: tx.ServiceCode
                ),
                ct
            );

            tx.MarkSubmitted(chain, network, submit.TxHash, submit.ExplorerUrl ?? request.ExplorerUrl);

            await _txRepo.SaveChangesAsync(ct);
            return ToDto(tx);
        }

        public async Task<TransactionDto> SettleAsync(Guid transactionId, CancellationToken ct = default)
        {
            var tx = await _txRepo.GetByIdForUpdateAsync(transactionId, ct);
            if (tx is null)
                throw new NotFoundException("Transaction not found.");

            if (tx.Status != TransactionStatus.Submitted)
                throw new ConflictException("Only Submitted transactions can be settled.");

            if (string.IsNullOrWhiteSpace(tx.BlockchainTxHash) || string.IsNullOrWhiteSpace(tx.Chain) || string.IsNullOrWhiteSpace(tx.Network))
                throw new ConflictException("Transaction has no on-chain submit data.");

            // Verify (fake for now)
            var verify = await _verifier.VerifyAsync(tx.Chain!, tx.Network!, tx.BlockchainTxHash!, ct);

            if (!verify.IsConfirmed)
                throw new ConflictException("Transaction is not confirmed on-chain yet.");

            if (!verify.IsSuccess)
            {
                tx.MarkFailed(verify.FailureReason ?? "ONCHAIN_FAILED");
                await _txRepo.SaveChangesAsync(ct);
                return ToDto(tx);
            }

            tx.MarkSettled();
            await _txRepo.SaveChangesAsync(ct);

            return ToDto(tx);
        }

        public async Task<TransactionDto> FailAsync(Guid transactionId, string? reason, CancellationToken ct = default)
        {
            var tx = await _txRepo.GetByIdForUpdateAsync(transactionId, ct);
            if (tx is null)
                throw new NotFoundException("Transaction not found.");

            if (tx.Status != TransactionStatus.Submitted)
                throw new ConflictException("Only Submitted transactions can be failed.");

            tx.MarkFailed(string.IsNullOrWhiteSpace(reason) ? "ONCHAIN_FAILED" : reason.Trim());
            await _txRepo.SaveChangesAsync(ct);

            return ToDto(tx);
        }

        private static TransactionDto ToDto(Transaction tx)
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
                tx.VendorId,
                tx.MarketplaceServiceId,
                tx.UnitPrice,
                tx.UnitPriceCurrency,
                tx.Chain,
                tx.Network,
                tx.BlockchainTxHash,
                tx.ExplorerUrl,
                tx.CreatedAtUtc,
                tx.SubmittedAtUtc,
                tx.SettledAtUtc,
                tx.FailedAtUtc,
                tx.FailureReason
            );
    }
}
