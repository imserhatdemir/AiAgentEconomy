using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Contracts.Wallets
{
    public sealed record CreateWalletForAgentRequest(
        string Address,
        string? Chain,
        string? Type
    );
}
