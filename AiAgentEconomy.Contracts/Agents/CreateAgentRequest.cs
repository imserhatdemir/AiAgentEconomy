using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Contracts.Agents
{
    public sealed record CreateAgentRequest(
    string Name,
    string? Description
    );
}
