using Identity.Domain.Entities;
using Identity.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Tests.Unit
{
    public class AuthorizationTests
    {
        [Fact]
        public void Should_GenerateToken_WithRoleClaim()
        {
            var user = new User("teste de nome", "test@email.com", "hash", "Admin");
            var tokenService = new TokenService("2f844838-6b5e-4656-9086-cca7fa971ef2");

            var token = tokenService.GenerateToken(user);

            Assert.False(string.IsNullOrEmpty(token));
            Assert.Contains(".", token); // JWT tem formato header.payload.signature
        }

    }
}
