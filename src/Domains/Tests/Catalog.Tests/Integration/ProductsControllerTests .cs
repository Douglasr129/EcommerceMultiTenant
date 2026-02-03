using Catalog.Application.Commands.ProductCommands;
using Catalog.Tests.Integration.Tools;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Catalog.Tests.Integration
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal PriceAmount { get; set; }
        public string PriceCurrency { get; set; }
        public int Stock { get; set; }
        public List<LinkDto> Links { get; set; }
    }

    public class LinkDto
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
    }

    public class ProductsControllerTests : BaseIntegrationTests
    {

        [Fact]
        public async Task Should_Generate_Exception_If_CategoryId_Does_Not_Match()
        {
            // Arrange
            string errorMessage = string.Empty;
            var userId = Guid.NewGuid().ToString();
            var token = AuthenticateClient(userId, "test@example.com", "Admin");

            var command = new CreateProductCommand
            {
                Name = "Produto Teste",
                Price = 5, 
                StockInicial = 10,
                CategoryId = Guid.NewGuid()
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/catalog/products")
            {
                Content = JsonContent.Create(command)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(content))
            {
                var jsonDocument = JsonDocument.Parse(content);
                errorMessage = jsonDocument.RootElement.GetProperty("error").GetString() ?? "";
            }
            // Assert
            Assert.Equal("Categoria não encontrado", errorMessage);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Sholde_Must_Successfully_Register_a_Product()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var token = AuthenticateClient(userId, "test@example.com", "Admin");

            var command = new CreateProductCommand
            {
                Name = "Produto Teste",
                Price = 5,
                StockInicial = 10
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/catalog/products")
            {
                Content = JsonContent.Create(command)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var productResponse = JsonSerializer.Deserialize<ProductResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });


            // Assert
            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(productResponse);
            Assert.Equal("Produto Teste", productResponse.Name);
            Assert.Equal(5.00m, productResponse.PriceAmount);
            Assert.Equal("BRL", productResponse.PriceCurrency);
        }
        [Fact]
        public async Task Should_Return_Product_By_Id()
        {
            var userId = Guid.NewGuid().ToString();
            var token = AuthenticateClient(userId, "test@example.com", "Admin");

            var command = new CreateProductCommand
            {
                Name = "Produto Teste",
                Price = 5,
                StockInicial = 10
            };

            var createRequest = new HttpRequestMessage(HttpMethod.Post, "/api/catalog/products")
            {
                Content = JsonContent.Create(command)
            };
            createRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var createResponse = await _httpClient.SendAsync(createRequest);
            var createdProduct = JsonSerializer.Deserialize<ProductResponse>(
                await createResponse.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            var getResponse = await _httpClient.GetAsync($"/api/catalog/products/{createdProduct!.Id}");
            var productResponse = JsonSerializer.Deserialize<ProductResponse>(
                await getResponse.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.Equal("Produto Teste", productResponse!.Name);
        }
        [Fact]
        public async Task Should_Return_Error_When_Ids_Do_Not_Match()
        {
            var userId = Guid.NewGuid().ToString();
            var token = AuthenticateClient(userId, "test@example.com", "Admin");

            var command = new UpdateProductCommand
            {
                ProductId = Guid.NewGuid(),
                Name = "Produto Atualizado",
                Price = 10
            };

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/catalog/products/{Guid.NewGuid()}")
            {
                Content = JsonContent.Create(command)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(content);
            var errorMessage = jsonDocument.RootElement.GetProperty("error").GetString();

            Assert.Equal("Os IDs não conferem", errorMessage);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task Should_Return_Forbidden_For_User_Role()
        {
            var userId = Guid.NewGuid().ToString();
            var token = AuthenticateClient(userId, "test@example.com", "User"); // role User

            var command = new CreateProductCommand
            {
                Name = "Produto Teste",
                Price = 5,
                StockInicial = 10
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/catalog/products")
            {
                Content = JsonContent.Create(command)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Without_Token()
        {
            var command = new CreateProductCommand
            {
                Name = "Produto Teste",
                Price = 5,
                StockInicial = 10
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/catalog/products")
            {
                Content = JsonContent.Create(command)
            };

            var response = await _httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task Should_Update_Product_Successfully()
        {
            var token = AuthenticateClient(Guid.NewGuid().ToString(), "test@example.com", "Admin");

            var command = new CreateProductCommand
            {
                Name = "Produto Original",
                Price = 5,
                StockInicial = 10
            };

            var createResponse = await _httpClient.PostAsJsonAsync("/api/catalog/products", command);
            var createdProduct = JsonSerializer.Deserialize<ProductResponse>(
                await createResponse.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            var updateCommand = new UpdateProductCommand
            {
                ProductId = createdProduct!.Id,
                Name = "Produto Atualizado",
                Price = 15
            };

            var updateResponse = await _httpClient.PutAsJsonAsync($"/api/catalog/products/{createdProduct.Id}", updateCommand);
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        }
        [Fact]
        public async Task Should_Delete_Product_Successfully()
        {
            var token = AuthenticateClient(Guid.NewGuid().ToString(), "test@example.com", "Admin");

            var command = new CreateProductCommand
            {
                Name = "Produto Para Excluir",
                Price = 5,
                StockInicial = 10
            };

            var createResponse = await _httpClient.PostAsJsonAsync("/api/catalog/products", command);
            var createdProduct = JsonSerializer.Deserialize<ProductResponse>(
                await createResponse.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            var deleteResponse = await _httpClient.DeleteAsync($"/api/catalog/products/{createdProduct!.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
        [Fact]
        public async Task Should_List_All_Products()
        {
            var token = AuthenticateClient(Guid.NewGuid().ToString(), "test@example.com", "Admin");

            var response = await _httpClient.GetAsync("/api/catalog/products");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("products", content);
        }
        [Fact]
        public async Task Should_Get_Products_By_Category()
        {
            var token = AuthenticateClient(Guid.NewGuid().ToString(), "test@example.com", "Admin");

            var categoryId = Guid.NewGuid();
            var response = await _httpClient.GetAsync($"/api/catalog/products/category/{categoryId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task Should_Update_Product_Stock_Successfully()
        {
            var userId = Guid.NewGuid().ToString();
            var token = AuthenticateClient(userId, "test@example.com", "Admin");

            var command = new CreateProductCommand
            {
                Name = "Produto Teste",
                Price = 5,
                StockInicial = 10
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/catalog/products")
            {
                Content = JsonContent.Create(command)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var createdProduct = JsonSerializer.Deserialize<ProductResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });


            // Atualiza estoque
            var amountToAdd = 3;
            var updateResponse = await _httpClient.PutAsync(
                        $"/api/catalog/products/product/{createdProduct!.Id}/addstock/{amountToAdd}",
                        null
                    );


            updateResponse.EnsureSuccessStatusCode();

            var updatedProduct = JsonSerializer.Deserialize<ProductResponse>(
                await updateResponse.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            // Valida se o estoque foi atualizado corretamente
            Assert.Equal(createdProduct.Stock + amountToAdd, updatedProduct!.Stock);
        }

    }
}
