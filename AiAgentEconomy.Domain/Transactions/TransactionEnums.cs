namespace AiAgentEconomy.Domain.Transactions
{
    public enum TransactionStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,

        // On-chain lifecycle
        Submitted = 3, // tx created & submitted to network
        Settled = 4,   // confirmed success
        Failed = 5     // confirmed failure
    }
}
