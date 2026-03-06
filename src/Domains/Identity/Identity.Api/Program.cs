using EvolveDb;
using Identity.Api.Configurations;
using Identity.Api.Middlewares;
using Identity.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCorsConfiguration();
builder.Services.AddControllers();
// PASSO 1: Adicionar serviços de documentação ANTES de tudo
// Swagger/OpenAPI precisa ser registrado para funcionar
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddOpenApi();           
// PASSO 2: Configuração do banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"]!;
// PASSO 3: Executar migrações com Evolve
// Validar se a string de conexão existe
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi configurada.");
}
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);
// PASSO 4: Registrar DbContext
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(connectionString));
// PASSO 5: Conexão com Rabbitmq
var rabbitMqConnectionString = builder.Configuration["RabbitMQ:ConnectionString"];
builder.Services.AddRabbitMqConnection(rabbitMqConnectionString!);
// PASSO 5: Registrar serviços da aplicação
builder.Services.AddDependencyInjection(jwtSecretKey);
// PASSO 6:  Configurar autorização com policies
builder.Services.AddAuthorizationConfiguration(builder.Configuration);
var app = builder.Build();
app.UseCors("AngularAppPolicy");
app.UseMiddleware<ExceptionHandlingMiddleware>();
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
    app.MapOpenApi();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
