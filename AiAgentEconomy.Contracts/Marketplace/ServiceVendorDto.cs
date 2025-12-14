using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Contracts.Marketplace
{
    public sealed record ServiceVendorDto(
        Guid Id,
        string Name,
        string Category,
        string WalletAddress,
        bool IsActive,
        DateTime CreatedAtUtc
    );
}
