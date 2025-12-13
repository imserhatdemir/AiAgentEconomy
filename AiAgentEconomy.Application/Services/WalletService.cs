using AiAgentEconomy.Application.Interfaces;
using AiAgentEconomy.Contracts.Wallets;
using AiAgentEconomy.Domain.Wallets;

namespace AiAgentEconomy.Application.Services
{
    public sealed class WalletService : IWalletService
    {
        private readonly IWalletRepository _repo;
        public WalletService(IWalletRepository repo) => _repo = repo;

        public async Task<WalletDto> CreateAsync(CreateWalletRequest request, CancellationToken ct = default)
        {
            var address = (request.Address ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address is required.");

            if (!address.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Address must start with 0x.");

            var existing = await _repo.GetByAddressAsync(address, ct);
            if (existing is not null)
                throw new InvalidOperationException("Wallet address already exists.");

            var chain = string.IsNullOrWhiteSpace(request.Chain) ? "Arbitrum" : request.Chain.Trim();

            var type = WalletType.NonCustodial;
            if (!string.IsNullOrWhiteSpace(request.Type) &&
                Enum.TryParse<WalletType>(request.Type, ignoreCase: true, out var parsed))
            {
                type = parsed;
            }

            var wallet = new Wallet(request.AgentId,
                                    chain,
                                    address,
                                    type)
                         {
                            Id = Guid.NewGuid(),
                            IsActive = true,
                            CreatedAtUtc = DateTime.UtcNow
                         };

            await _repo.AddAsync(wallet, ct);
            await _repo.SaveChangesAsync(ct);

            return new WalletDto(wallet.Id, wallet.AgentId, wallet.Chain, wallet.Address, wallet.Type.ToString(), wallet.IsActive, wallet.CreatedAtUtc);
        }

        public async Task<WalletDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var w = await _repo.GetByIdAsync(id, ct);
            return w is null
                ? null
                : new WalletDto(w.Id, w.AgentId, w.Chain, w.Address, w.Type.ToString(), w.IsActive, w.CreatedAtUtc);
        }
    }
}
