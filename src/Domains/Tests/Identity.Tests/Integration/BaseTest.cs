using Identity.Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Identity.Tests.Integration
{
    public abstract class BaseTest : IAsyncLifetime
    {
        public readonly PostgreSqlContainer _dbContainer;
        public WebApplicationFactory<Program> _factory;
        public HttpClient _client;

        protected BaseTest()
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

            _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove o DbContext existente
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<IdentityDbContext>));

                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Adiciona o DbContext de teste
                    services.AddDbContext<IdentityDbContext>(options =>
                    {
                        options.UseNpgsql(_dbContainer.GetConnectionString());
                    });

                    // Garante que o banco está criado
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                    db.Database.EnsureCreated();
                });
            });

            _client = _factory.CreateClient();
        }
        public async Task DisposeAsync()
        {
            _client?.Dispose();
            if (_factory != null)
                await _factory.DisposeAsync();
            await _dbContainer.DisposeAsync();
        }
    }
}
