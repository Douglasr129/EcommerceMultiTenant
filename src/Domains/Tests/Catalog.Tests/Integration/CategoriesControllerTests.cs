using Catalog.Api.Models;
using Catalog.Application.Commands.CategoryCommands;
using Catalog.Tests.Integration.Tools;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Catalog.Tests.Integration
{
    public class CategoriesControllerTests : BaseIntegrationTests
    {
        public CategoriesControllerTests()
        {
            // Aqui você inicializa seu TestServer e HttpClient
            // Exemplo: _httpClient = _factory.CreateClient();
        }

        [Fact]
        public async Task Should_Create_Category_Successfully()
        {
            var token = AuthenticateClient(Guid.NewGuid().ToString(), "test@example.com", "Admin");

            var command = new CreateCategoryCommand
            {
                Name = "Categoria Teste"
            };

            var response = await _httpClient.PostAsJsonAsync("/api/catalog/categories", command);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdCategory = JsonSerializer.Deserialize<CategoryResponse>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            Assert.Equal("Categoria Teste", createdCategory!.Name);
        }

        [Fact]
        public async Task Should_List_All_Categories()
        {
            var token = AuthenticateClient(Guid.NewGuid().ToString(), "test@example.com", "Admin");

            var response = await _httpClient.GetAsync("/api/catalog/categories");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("categorys", content); // valida estrutura
        }

        [Fact]
        public async Task Should_Get_Category_By_Id()
        {
            var token = AuthenticateClient(Guid.NewGuid().ToString(), "test@example.com", "Admin");

            // cria categoria primeiro
            var command = new CreateCategoryCommand { Name = "Categoria Para Buscar" };
            var createResponse = await _httpClient.PostAsJsonAsync("/api/catalog/categories", command);
            var createdCategory = JsonSerializer.Deserialize<CategoryResponse>(
                await createResponse.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            var response = await _httpClient.GetAsync($"/api/catalog/categories/{createdCategory!.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var category = JsonSerializer.Deserialize<CategoryResponse>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            Assert.Equal("Categoria Para Buscar", category!.Name);
        }

        [Fact]
        public async Task Should_Update_Category_Successfully()
        {
            var token = AuthenticateClient(Guid.NewGuid().ToString(), "test@example.com", "Admin");

            // cria categoria
            var command = new CreateCategoryCommand { Name = "Categoria Original" };
            var createResponse = await _httpClient.PostAsJsonAsync("/api/catalog/categories", command);
            var createdCategory = JsonSerializer.Deserialize<CategoryResponse>(
                await createResponse.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            // atualiza categoria
            var updateCommand = new UpdateCategoryCommand
            {
                CategoryId = createdCategory!.Id,
                Name = "Categoria Atualizada"
            };

            var updateResponse = await _httpClient.PutAsJsonAsync($"/api/catalog/categories/{createdCategory.Id}", updateCommand);
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            var updatedCategory = JsonSerializer.Deserialize<CategoryResponse>(
                await updateResponse.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            Assert.Equal("Categoria Atualizada", updatedCategory!.Name);
        }

        [Fact]
        public async Task Should_Delete_Category_Successfully()
        {
            var token = AuthenticateClient(Guid.NewGuid().ToString(), "test@example.com", "Admin");

            // cria categoria
            var command = new CreateCategoryCommand { Name = "Categoria Para Excluir" };
            var createResponse = await _httpClient.PostAsJsonAsync("/api/catalog/categories", command);
            var createdCategory = JsonSerializer.Deserialize<CategoryResponse>(
                await createResponse.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            // exclui categoria
            var deleteResponse = await _httpClient.DeleteAsync($"/api/catalog/categories/{createdCategory!.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}
