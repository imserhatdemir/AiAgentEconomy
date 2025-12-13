using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Transactions
{
    public enum TransactionStatus
    {
        Pending = 1,        // Created, waiting for execution
        Approved = 2,       // Passed policy & budget
        Rejected = 3,       // Blocked by rules
        OnChainSent = 4,    // Sent to blockchain
        Confirmed = 5,      // Confirmed on-chain
        Failed = 6          // Execution failed
    }
}
