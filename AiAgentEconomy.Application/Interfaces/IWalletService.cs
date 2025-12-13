using AiAgentEconomy.Contracts.Wallets;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface IWalletService
    {
        Task<WalletDto> CreateForAgentAsync(Guid agentId, CreateWalletForAgentRequest request, CancellationToken ct = default);
        Task<WalletDto?> GetByAgentIdAsync(Guid agentId, CancellationToken ct = default);
    }
}
