using Catalog.Api.Configurations;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace Catalog.Tests.Integration.Tools
{
    public abstract class BaseIntegrationTests : IAsyncLifetime
    {
        protected PostgreSqlContainer _dbContainer { get; private set; }
        protected RabbitMqContainer _rabbitMqContainer { get; private set; }
        protected CustomWebApplicationFactory<Program> _factory { get; private set; }
        protected JwtTokenGenerator _tokenGenerator { get; private set; }

        protected HttpClient _httpClient { get; private set; }
        protected IConnection _rabbitMqConnection { get; private set; }
        protected string DatabaseConnectionString => _dbContainer.GetConnectionString();
        protected string RabbitMqConnectionString => _rabbitMqContainer.GetConnectionString();
        protected const string TestJwtSecret = "super_secret_key_ecdd135a-176e-4f56-b2df-55de22256dcf";
        protected BaseIntegrationTests()
        {
            _dbContainer = new PostgreSqlBuilder()
                .WithDatabase("testdb")
                .WithUsername("admin")
                .WithPassword("admin123")
                .WithImage("postgres:latest")
                .Build();

            _rabbitMqContainer = new RabbitMqBuilder()
                .WithUsername("TesteRabbit")
                .WithPassword("TesteRabbit")
                .WithImage("rabbitmq:3-management")
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            EvolveConfig.ExecuteMigrations(DatabaseConnectionString);
            await _rabbitMqContainer.StartAsync();
            _rabbitMqConnection = RabbitMqConfig.Connect(RabbitMqConnectionString);
            var jwt = new JwtConfiguration()
            {
                SecretKey = TestJwtSecret,
                Audience = "EcommerceUsers",
                Issuer = "Ecommerce"
            };
            _tokenGenerator = new JwtTokenGenerator(jwt);

            _factory = new CustomWebApplicationFactory<Program>(DatabaseConnectionString, RabbitMqConnectionString, jwt);
            
            _httpClient = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    BaseAddress = new Uri("http://localhost")
                }
             );
        }

        public async Task DisposeAsync()
        {
            _httpClient?.Dispose();

            if (_rabbitMqConnection != null)
            {
                await _rabbitMqConnection.CloseAsync();
                _rabbitMqConnection.Dispose();
            }

            if (_factory != null)
                await _factory.DisposeAsync();

            // Dispose dos containers em paralelo
            await Task.WhenAll(
                _dbContainer.DisposeAsync().AsTask(),
                _rabbitMqContainer.DisposeAsync().AsTask()
            );
        }
        protected string AuthenticateClient(string userId, string email, string role = "User")
        {
            var token = _tokenGenerator.GenerateToken(userId, email, role);
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return token;
        }
    }
}
