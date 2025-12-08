using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Entities
{
    public class ServiceVendor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }          // Örn: "DataAPI.io"
        public string Category { get; set; }      // "Data API", "AI API", "SaaS"
        public string WalletAddress { get; set; } // Arbitrum cüzdan adresi
        public decimal Price { get; set; }        // Ör: 5 USDC
        public string Currency { get; set; }      // "USDC"
    }

}
