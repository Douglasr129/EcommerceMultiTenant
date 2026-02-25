using RabbitMQ.Client;

namespace Identity.Api.Configurations
{
    public static class RabbitMqConfig
    {
        public static IServiceCollection AddRabbitMqConnection(
            this IServiceCollection services,
            string rabbitMqConnectionString)
        {
            services.AddSingleton<IConnection>(sp =>
            {
                try
                {
                    return Connect(rabbitMqConnectionString!);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao conectar com RabbitMQ: {ex.Message}");
                    throw;
                }
            });

            return services;
        }
        public static IConnection Connect(string connectionString)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(connectionString)
            };
            var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            Console.WriteLine($"Conexão com RabbitMQ estabelecida: {factory.HostName}:{factory.Port}");
            return connection;

        }
    }
}
