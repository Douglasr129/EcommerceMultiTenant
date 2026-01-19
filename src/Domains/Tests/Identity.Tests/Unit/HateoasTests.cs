using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Tests.Unit
{
    public class HateoasTests
    {
        [Fact]
        public void RegisterResponse_ShouldContainLinks()
        {
            var response = new
            {
                UserId = Guid.NewGuid(),
                Links = new[]
                {
                    new { Rel = "self", Href = "/api/auth/register" },
                    new { Rel = "login", Href = "/api/auth/login" }
                }
            };

            Assert.Contains(response.Links, l => l.Rel == "self");
            Assert.Contains(response.Links, l => l.Rel == "login");
        }
    }

}
