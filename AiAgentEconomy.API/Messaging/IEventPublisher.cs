namespace AiAgentEconomy.API.Messaging
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default);
    }
}
