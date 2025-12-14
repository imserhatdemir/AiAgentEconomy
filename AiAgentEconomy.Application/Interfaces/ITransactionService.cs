using AiAgentEconomy.Contracts.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDto> CreateForAgentAsync(Guid agentId, CreateTransactionRequest request, CancellationToken ct = default);
        Task<IReadOnlyList<TransactionDto>> GetByAgentAsync(Guid agentId, int take = 50, CancellationToken ct = default);
        Task<TransactionDto> SubmitAsync(Guid transactionId, SubmitTransactionRequest request, CancellationToken ct = default);
        Task<TransactionDto> SettleAsync(Guid transactionId, CancellationToken ct = default);
        Task<TransactionDto> FailAsync(Guid transactionId, FailTransactionRequest request, CancellationToken ct = default);
    }
}
