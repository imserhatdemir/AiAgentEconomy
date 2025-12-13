using AiAgentEconomy.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Marketplace
{
    public class ServiceVendor : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        // Vendor receives funds here
        public string WalletAddress { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public ICollection<MarketplaceService> Services { get; set; } = new List<MarketplaceService>();
    }
}
