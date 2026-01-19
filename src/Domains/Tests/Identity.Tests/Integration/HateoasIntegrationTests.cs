using Identity.Application.Commands;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Configurations;
using Identity.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace Identity.Tests.Integration
{
    public class HateoasIntegrationTests : BaseTest
    {
        [Fact]
        public async Task LoginResponse_ShouldContainLinks()
        {

            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            var hasher = new PasswordHasher();

            var user = new User("admin@test.com", hasher.Hash("123456"), "Admin");
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var command = new LoginUserCommand
            {
                Email = "admin@test.com",
                Password = "123456"
            };
            var response = await _client.PostAsJsonAsync("/api/auth/login", command);
            var content = await response.Content.ReadAsStringAsync();
            var jsonContent = JObject.Parse(content);

            var links = jsonContent["links"] as JArray;

            Assert.NotNull(links);
            Assert.Contains(links, l => l["rel"]?.ToString() == "self");
            Assert.Contains(links, l => l["rel"]?.ToString() == "admin-only");
        }
    }

}
