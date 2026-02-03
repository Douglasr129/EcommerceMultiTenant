using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Catalog.Tests.Integration.Tools
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private readonly string _connectionStringDb;
        private readonly string _connectionStringRabbitMq;
        private readonly JwtConfiguration _jwt;

        public CustomWebApplicationFactory(string connectionStringDb, string connectionStringRabbitMq, JwtConfiguration jwt)
        {
            _connectionStringDb = connectionStringDb;
            _jwt = jwt;
            _connectionStringRabbitMq = connectionStringRabbitMq;
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var dict = new Dictionary<string, string>
                {
                    {
                        "ConnectionStrings:DefaultConnection", _connectionStringDb
                    },
                    {
                        "Jwt:SecretKey", _jwt.SecretKey
                    },
                    {
                        "Jwt:Issuer", _jwt.Issuer
                    },
                    {
                        "Jwt:Audience", _jwt.Audience
                    },
                    {
                        "RabbitMQ:ConnectionString", _connectionStringRabbitMq
                    }
                };
                config.AddInMemoryCollection(dict!);
            });
        }
    }
    public class JwtConfiguration
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
