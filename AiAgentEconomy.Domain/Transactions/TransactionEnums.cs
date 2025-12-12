using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Transactions
{
    public enum AgentTransactionStatus
    {
        Pending = 1,
        OnChainSubmitted = 2,
        Confirmed = 3,
        Failed = 4,
        Cancelled = 5
    }
}
