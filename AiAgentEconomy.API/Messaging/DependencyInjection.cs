using RabbitMQ.Client;

namespace AiAgentEconomy.API.Messaging
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRabbitMqPublisher(this IServiceCollection services, IConfiguration cfg)
        {
            services.AddSingleton<IConnection>(_ =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = cfg["RabbitMq:Host"] ?? "localhost",
                    Port = int.Parse(cfg["RabbitMq:Port"] ?? "5672"),
                    UserName = cfg["RabbitMq:User"] ?? "guest",
                    Password = cfg["RabbitMq:Pass"] ?? "guest"
                };

                return factory.CreateConnection("AiAgentEconomy.API");
            });

            services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();

            return services;
        }
    }
}
