using Identity.Application.Commands;
using Identity.Application.Handlers;
using Identity.Domain.Entities;
using Identity.Infrastructure.Configurations;
using Identity.Infrastructure.Repisitories;
using Identity.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Identity.Tests.Integration
{
    public class LoginIntegrationTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer;
        private IdentityDbContext _context;

        public LoginIntegrationTests()
        {
            _dbContainer = new PostgreSqlBuilder()
                .WithDatabase("testdb")
                .WithUsername("admin")
                .WithPassword("admin123")
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
        }
        public async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
            await _context.DisposeAsync();
        }
        [Fact]
        public async Task Should_LoginSuccessfully_AndGenerateToken()
        {
            // Arrange
            var repo = new UserRepository(_context);
            var hasher = new PasswordHasher();
            var tokenService = new TokenService("2f844838-6b5e-4656-9086-cca7fa971ef2");

            // Criar usuário manualmente
            var user = new User("teste de nome", "integration@test.com", hasher.Hash("123456"), "Customer");
            await repo.AddAsync(user);

            var handler = new LoginUserHandler(repo, hasher, tokenService);

            var command = new LoginUserCommand
            {
                Email = "integration@test.com",
                Password = "123456"
            };

            // Act
            var token = await handler.Handle(command);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
            Assert.Contains(".", token); // JWT tem formato com pontos (header.payload.signature)
        }

    }
}
