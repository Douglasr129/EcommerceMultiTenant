using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;
using System.Text.Json;

namespace Catalog.Infrastructure.Messaging
{
    public class OrderPlacedConsumer(IConnection connection)
    {
        private readonly IConnection _connection = connection;

        public async Task Start()
        {
            using var channel = await _connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "order_placed", durable: true, exclusive: false, autoDelete: false,
                             arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var order = JsonSerializer.Deserialize<dynamic>(message);
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queue: "order_placed", autoAck: true, consumer: consumer);

            // Manter o consumidor rodando (importante!)
            await Task.Delay(Timeout.Infinite);
        }
    }
}
