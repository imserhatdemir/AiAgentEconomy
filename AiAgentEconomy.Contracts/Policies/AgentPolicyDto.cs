using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Contracts.Policies
{
    public sealed record AgentPolicyDto(
        Guid Id,
        Guid AgentId,
        string Name,
        bool IsActive,
        decimal MaxPerTransaction,
        decimal DailyLimit,
        string Currency,
        string? AllowedVendorsCsv,
        string? AllowedServicesCsv,
        DateOnly? DailyWindowDate,
        decimal SpentInDailyWindow,
        DateTime CreatedAtUtc
    );
}
