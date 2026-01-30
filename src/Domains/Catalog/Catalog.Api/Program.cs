using Catalog.Api.Configurations;
using Catalog.Api.Middlewares;
using Catalog.Api.Services;
using Catalog.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// PASSO 1: Adicionar serviços de documentação ANTES de tudo
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

// PASSO 2: Configurar autenticação JWT
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerConfiguration();
builder.Services.AddOpenApi();      

// PASSO 3: Executar migrações com Evolve
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Validar se a string de conexão existe
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi configurada.");
}
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);

// PASSO 4: Registrar DbContext
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseNpgsql(connectionString));

// PASSO 5: Conexão com Rabbitmq
var rabbitMqConnectionString = builder.Configuration["RabbitMQ:ConnectionString"];
builder.Services.AddRabbitMqConnection(rabbitMqConnectionString!);

// PASSO 6: Adicionar todas as dependências
builder.Services.AddDependencyInjection();

// PASSO 7: Configurar autorização com policies
builder.Services.AddAuthorizationConfiguration(builder.Configuration);

// PASSO 8: Construir a aplicação
var app = builder.Build();

// Middleware global de tratamento de erros
app.UseGlobalErrorHandler();


if (app.Environment.IsDevelopment())
{
    // OpenAPI endpoint
    app.MapOpenApi();

    // Swagger UI
    app.UseSwaggerConfiguration();
    // Scalar
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Ecommerce Multi-Tenant API")
            .WithTheme(ScalarTheme.Purple)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithOpenApiRoutePattern("/swagger/v1/swagger.json"); // Rota OpenAPI
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
