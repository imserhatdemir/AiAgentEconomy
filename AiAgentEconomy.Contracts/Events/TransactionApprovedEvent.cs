using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Contracts.Events
{
    public sealed record TransactionApprovedEvent(
         Guid TransactionId,
         Guid AgentId,
         decimal Amount,
         string Currency,
         string CorrelationId,
         DateTimeOffset OccurredAt
     );
}
