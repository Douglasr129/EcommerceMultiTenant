using Catalog.Api.Configurations;
using Catalog.Api.Middlewares;
using Catalog.Api.Services;
using Catalog.Application.Handlers.CategoryHandlers;
using Catalog.Application.Handlers.ProductHandlers;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Configurations;
using Catalog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// PASSO 1: Adicionar serviços de documentação ANTES de tudo
// Swagger/OpenAPI precisa ser registrado para funcionar
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddEndpointsApiExplorer(); // Necessário para OpenAPI descobrir endpoints
builder.Services.AddSwaggerGen(options => // Gera a documentação Swagger
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ecommerce Multi-Tenant API",
        Version = "v1",
        Description = "API de E-commerce Multi-Tenant"
    });
});
// Gera a documentação Swagger
builder.Services.AddOpenApi();              // Gera o OpenAPI (v1.0)

// PASSO 2: Configuração do banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Validar se a string de conexão existe
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi configurada.");
}

// PASSO 2.1: Configuração do JWT
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "Ecommerce";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "EcommerceUsers";

// Validar se a chave JWT existe
if (string.IsNullOrEmpty(jwtSecretKey))
{
    throw new InvalidOperationException("A configuração 'Jwt:SecretKey' não foi encontrada. Verifique o appsettings.json ou variáveis de ambiente.");
}

var key = Encoding.UTF8.GetBytes(jwtSecretKey);

// PASSO 3: Executar migrações com Evolve
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);

// PASSO 4: Registrar DbContext
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseNpgsql(connectionString));

// PASSO 5: Conexão com Rabbitmq
var rabbitMqConnectionString = builder.Configuration["RabbitMQ:ConnectionString"];
builder.Services.AddRabbitMqConnection(rabbitMqConnectionString!);


// PASSO 5: Registrar serviços da aplicação

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddTransient(sp => new DeleteProductHandler(sp.GetRequiredService<IProductRepository>()));
builder.Services.AddTransient(sp => new CreateProductHandler(sp.GetRequiredService<IProductRepository>()));
builder.Services.AddTransient(sp => new GetProductAllHandler(sp.GetRequiredService<IProductRepository>()));
builder.Services.AddTransient(sp => new GetProductByIdHandler(sp.GetRequiredService<IProductRepository>()));
builder.Services.AddTransient(sp => new GetProductsByCategoryHandler(sp.GetRequiredService<IProductRepository>()));
builder.Services.AddTransient(sp => new UpdateProductHandler(sp.GetRequiredService<IProductRepository>()));
builder.Services.AddTransient(sp => new UpdateStockHandler(sp.GetRequiredService<IProductRepository>()));
builder.Services.AddTransient(sp => new UpdateProductsByCategoryHandler(sp.GetRequiredService<IProductRepository>()));


builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient(sp => new CreateCategoryHandler(sp.GetRequiredService<ICategoryRepository>()));
builder.Services.AddTransient(sp => new DeleteCategoryHandler(sp.GetRequiredService<ICategoryRepository>()));
builder.Services.AddTransient(sp => new GetCategoryAllHandler(sp.GetRequiredService<ICategoryRepository>()));
builder.Services.AddTransient(sp => new GetCategoryByIdHandler(sp.GetRequiredService<ICategoryRepository>()));
builder.Services.AddTransient(sp => new UpdateCategoryHandler(sp.GetRequiredService<ICategoryRepository>()));



// PASSO 6: Configurar autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "Ecommerce",
        ValidAudience = "EcommerceUsers",
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// PASSO 7: Configurar autorização com policies
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"))
    .AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"))
    .AddPolicy("SellerPolicy", policy => policy.RequireRole("Seller"))
    .AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer"));


// PASSO 8: Construir HATEOAS
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ILinkService, LinkService>();

// PASSO 9: Construir a aplicação
var app = builder.Build();

// Middleware global de tratamento de erros
app.UseGlobalErrorHandler();


if (app.Environment.IsDevelopment())
{
    // OpenAPI endpoint
    app.MapOpenApi();

    // Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce Multi-Tenant API v1");
        options.RoutePrefix = "swagger"; // http://localhost:5000/swagger
    });

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
