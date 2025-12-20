using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace AiAgentEconomy.API.Messaging
{
    public sealed class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IConnection _connection;
        private readonly string _exchange;

        public RabbitMqEventPublisher(IConnection connection, IConfiguration cfg)
        {
            _connection = connection;
            _exchange = cfg["RabbitMq:Exchange"] ?? "aiagenteconomy.events";
        }

        public Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default)
        {
            using var channel = _connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: _exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var props = channel.CreateBasicProperties();
            props.ContentType = "application/json";
            props.DeliveryMode = 2; // persistent

            channel.BasicPublish(
                exchange: _exchange,
                routingKey: routingKey,
                basicProperties: props,
                body: body);

            return Task.CompletedTask;
        }
    }
}
