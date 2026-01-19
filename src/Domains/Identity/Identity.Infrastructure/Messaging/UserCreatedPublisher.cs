using Identity.Domain.Entities;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Identity.Infrastructure.Messaging
{
    public class UserCreatedPublisher(IConnection connection)
    {
        private readonly IConnection _connection = connection;

        public async void Publish(User user)
        {

            using var channel = await _connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "user_created", durable: true, exclusive: false, autoDelete: false,
                             arguments: null);
            var message = JsonSerializer.Serialize(new { user.Id, user.Email, user.Role });
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "user_created", body: body);
        }
    }
}
