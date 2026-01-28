using Catalog.Application.Commands;
using Catalog.Domain.Entities;
using Catalog.Tests.Integration.Tools;
using System.Net;
using System.Net.Http.Json;
using System.Xml.Linq;

namespace Catalog.Tests.Integration
{
    public class ProductsControllerTests : BaseIntegrationTests
    {
        [Fact]
        public async Task AddProduct_DeveCriarProdutoERetornarCreated()
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

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
        }

    }
}
