using AiAgentEconomy.Contracts.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Application.Interfaces
{
    public interface ITransactionLifecycleService
    {
        Task<TransactionDto> SubmitAsync(Guid transactionId, SubmitTransactionRequest request, CancellationToken ct = default);
        Task<TransactionDto> SettleAsync(Guid transactionId, CancellationToken ct = default);
        Task<TransactionDto> FailAsync(Guid transactionId, string? reason, CancellationToken ct = default);
    }
}
