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
    public class ProductsControllerTests : BaseIntegrationTests
    {
        private string _adminToken;
        private string _managerToken;

        public ProductsControllerTests()
        {
            // Gere os tokens no setup ou use um método auxiliar
            _adminToken = GenerateToken("Admin");
            _managerToken = GenerateToken("Manager");
        }

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
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/catalog/products")
            {
                Content = JsonContent.Create(command)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(content);
            var errorMessage = jsonDocument.RootElement.GetProperty("error").GetString();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Categoria não encontrado", errorMessage);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Without_Token()
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
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private string GenerateToken(string role)
        {
            // Implemente conforme sua estratégia de autenticação
            // Exemplo com JWT:
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("sua-chave-secreta-aqui");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
