using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Contracts.Marketplace
{
    public sealed record MarketplaceServiceDto(
        Guid Id,
        Guid VendorId,
        string ServiceCode,
        string Name,
        decimal Price,
        string Currency,
        bool IsActive,
        DateTime CreatedAtUtc
    );
}
