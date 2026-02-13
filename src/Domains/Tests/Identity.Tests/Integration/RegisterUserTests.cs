using Identity.Application.Commands;
using Identity.Application.Handlers;
using Identity.Infrastructure.Configurations;
using Identity.Infrastructure.Messaging;
using Identity.Infrastructure.Repisitories;
using Identity.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;



namespace Identity.Tests.Integration
{
    public class RegisterUserTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer;
        private IdentityDbContext _context;
        private readonly RabbitMqContainer _rabbitMqContainer;
        private IConnection _connection;


        public RegisterUserTests()
        {
            _dbContainer = new PostgreSqlBuilder()
                .WithDatabase("testdb")
                .WithUsername("admin")
                .WithPassword("admin123")
                .Build();

            _rabbitMqContainer = new RabbitMqBuilder()
                .WithUsername("guest")
                .WithPassword("guest")
                .Build();

        }
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            var options = new DbContextOptionsBuilder<IdentityDbContext>()
                .UseNpgsql(_dbContainer.GetConnectionString())
                .Options;
            _context = new IdentityDbContext(options);
            await _context.Database.EnsureCreatedAsync();

            await _rabbitMqContainer.StartAsync();
            var factory = new ConnectionFactory() { Uri = new Uri(_rabbitMqContainer.GetConnectionString()) };
            _connection = await factory.CreateConnectionAsync();

        }
        public async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
            await _context.DisposeAsync();
        }

    }
}
