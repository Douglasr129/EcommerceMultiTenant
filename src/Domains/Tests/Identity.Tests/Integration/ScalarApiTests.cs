using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Identity.Tests.Integration
{
    public class ScalarApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ScalarApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ScalarEndpoint_ShouldBeAccessible()
        {
            var response = await _client.GetAsync("/scalar");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }

}
