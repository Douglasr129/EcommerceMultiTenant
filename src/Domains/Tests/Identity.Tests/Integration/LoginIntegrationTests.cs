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

    }
}
