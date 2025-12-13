using AiAgentEconomy.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Marketplace
{
    public class MarketplaceService : AuditableEntity
    {
        public Guid VendorId { get; set; }
        public ServiceVendor Vendor { get; set; } = default!;

        public string ServiceCode { get; set; } = string.Empty;   // e.g. "Service1"
        public decimal Price { get; set; }                        // e.g. 5
        public string Currency { get; set; } = "USDC";

        public bool IsActive { get; set; } = true;
    }
}
