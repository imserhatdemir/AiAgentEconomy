using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Wallets
{
    public enum WalletType
    {
        NonCustodial = 0,
        Custodial = 1,
        SmartContract = 2
    }
}
