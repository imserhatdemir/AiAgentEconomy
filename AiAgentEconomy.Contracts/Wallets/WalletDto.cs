using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Contracts.Wallets
{
    public sealed record WalletDto(
        Guid Id,
        Guid AgentId,
        string Chain,
        string Address,
        string Type,
        bool IsActive,
        DateTime CreatedAtUtc
    );
}
