using Catalog.Application.Commands.ProductCommands;
using Catalog.Tests.Integration.Tools;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Catalog.Tests.Integration
{
    public class ProductsControllerTests : BaseIntegrationTests
    {
        [Fact]
        public async Task Should_Generate_Exception_If_CategoryId_Does_Not_Match()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Produto Teste",
                Price = 5,
                StockInicial = 10,
                CategoryId = Guid.NewGuid()
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/catalog/products", command);
            var content = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(content);
            var errorMessage = jsonDocument.RootElement.GetProperty("error").GetString();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Categoria não encontrado", errorMessage);
        }
    }
}
