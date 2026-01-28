using EvolveDb;
using Npgsql;

namespace Catalog.Api.Configurations
{
    public static class EvolveConfig
    {
        public static IServiceCollection AddEvolveConfiguration(
          this IServiceCollection services,
          IConfiguration configuration,
          IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                var connectionString = configuration[
                    "ConnectionStrings:DefaultConnection"];

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentNullException(
                        "Connection string 'DefaultConnection' not found.");
                }

                try
                {
                    ExecuteMigrations(connectionString);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return services;
        }
        public static void ExecuteMigrations(string connectionString)
        {
            using var evolveConnection = new NpgsqlConnection(connectionString);
            var evolve = new Evolve(
                evolveConnection)
            {
                Locations = ["../Catalog.Infrastructure/Migrations"],
                IsEraseDisabled = true
            };
            evolve.Migrate();
        }
    }
}
