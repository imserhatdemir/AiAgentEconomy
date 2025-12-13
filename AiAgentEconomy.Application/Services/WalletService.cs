using AiAgentEconomy.Application.Exceptions;
using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Wallets;
using AiAgentEconomy.Domain.Wallets;

namespace AiAgentEconomy.Application.Services
{
    public sealed class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepo;
        private readonly IAgentRepository _agentRepo;

        public WalletService(IWalletRepository walletRepo, IAgentRepository agentRepo)
        {
            _walletRepo = walletRepo;
            _agentRepo = agentRepo;
        }

        public async Task<WalletDto> CreateForAgentAsync(Guid agentId, CreateWalletForAgentRequest request, CancellationToken ct = default)
        {
            // 1) Agent var mı?
            var agent = await _agentRepo.GetByIdAsync(agentId, ct);
            if (agent is null)
                throw new NotFoundException("Agent not found.");

            // 2) Agent zaten wallet’a sahip mi?
            var existing = await _walletRepo.GetByAgentIdAsync(agentId, ct);
            if (existing is not null)
                throw new ConflictException("Agent already has a wallet.");

            // 3) Validation
            var address = (request.Address ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(address))
                throw new ValidationException("Address is required.");

            if (!address.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                throw new ValidationException("Address must start with 0x.");

            var chain = string.IsNullOrWhiteSpace(request.Chain) ? "Arbitrum" : request.Chain.Trim();

            // 4) Address uniqueness (chain+address)
            var exists = await _walletRepo.ExistsByChainAndAddressAsync(chain, address, ct);
            if (exists)
                throw new ConflictException("Wallet address already exists on this chain.");

            // 5) Type parse
            var type = WalletType.NonCustodial;
            if (!string.IsNullOrWhiteSpace(request.Type) &&
                Enum.TryParse<WalletType>(request.Type, ignoreCase: true, out var parsed))
            {
                type = parsed;
            }

            // 6) Create
            var wallet = new Wallet(agentId, chain, address, type);

            await _walletRepo.AddAsync(wallet, ct);
            await _walletRepo.SaveChangesAsync(ct);

            return new WalletDto(
                wallet.Id,
                wallet.AgentId,
                wallet.Chain,
                wallet.Address,
                wallet.Type.ToString(),
                wallet.IsActive,
                wallet.CreatedAtUtc
            );
        }

        public async Task<WalletDto?> GetByAgentIdAsync(Guid agentId, CancellationToken ct = default)
        {
            var wallet = await _walletRepo.GetByAgentIdAsync(agentId, ct);
            return wallet is null
                ? null
                : new WalletDto(wallet.Id, wallet.AgentId, wallet.Chain, wallet.Address, wallet.Type.ToString(), wallet.IsActive, wallet.CreatedAtUtc);
        }
    }
}
