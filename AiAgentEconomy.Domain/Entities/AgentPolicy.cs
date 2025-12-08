using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Domain.Entities
{
    public class AgentPolicy
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public string Key { get; set; }           // "MaxPerTx", "AllowedVendors", "MaxSlippage" vb.
        public string Value { get; set; }
    }

}
