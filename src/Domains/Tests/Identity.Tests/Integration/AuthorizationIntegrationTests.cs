using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Configurations;
using Identity.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Testcontainers.PostgreSql;

namespace Identity.Tests.Integration
{
    public class AuthorizationIntegrationTests : BaseTest
    {

        [Fact]
        public async Task Admin_Should_Access_AdminEndpoint()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            var hasher = new PasswordHasher();

            var user = new User("admin@test.com", hasher.Hash("123456"), "Admin");
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
            var token = tokenService.GenerateToken(user);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/auth/admin-only");
            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Customer_Should_Not_Access_AdminEndpoint()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            var hasher = new PasswordHasher();

            var user = new User("customer@test.com", hasher.Hash("123456"), "Customer");
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
            var token = tokenService.GenerateToken(user);

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/api/auth/admin-only");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }
    }

}
