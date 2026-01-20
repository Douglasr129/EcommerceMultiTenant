using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Catalog.Infrastructure.Messaging
{
    public class ProductCreatedPublisher(IConnection connection)
    {
        private readonly IConnection _connection = connection;

        public async void Publish(Product product) 
        {

            using var channel = await _connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "product_created", durable: true, exclusive: false, autoDelete: false,
                             arguments: null);
            var message = JsonSerializer.Serialize(new { product.Id, product.Name, product.Price, product.Stock, product.CategoryId });
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "product_created", body: body);
        }
    }
}
