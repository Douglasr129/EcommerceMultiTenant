using Catalog.Application.Handlers;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Configurations;
using Catalog.Infrastructure.Repositories;
using EvolveDb;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Npgsql;
using RabbitMQ.Client;
using Scalar.AspNetCore;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// PASSO 1: Adicionar serviços de documentação ANTES de tudo
// Swagger/OpenAPI precisa ser registrado para funcionar
builder.Services.AddControllers();
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
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"]!;
var key = Encoding.UTF8.GetBytes(jwtSecretKey);

// PASSO 3: Executar migrações com Evolve
using var cnx = new NpgsqlConnection(connectionString);
cnx.Open();
Console.WriteLine("Conexão com o banco estabelecida com sucesso!");
cnx.Close();

var evolve = new Evolve(cnx, msg => Console.WriteLine(msg))
{
    Locations = ["../Catalog.Infrastructure/Migrations"],
    IsEraseDisabled = true
};

evolve.Migrate();
Console.WriteLine("Migrações executadas com sucesso!");
cnx.Close();

// PASSO 4: Registrar DbContext
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseNpgsql(connectionString));

// PASSO 5: Conexão com Rabbitmq

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory
    {
        HostName = "localhost",
        Port = 5672,
        UserName = "admin",
        Password = "admin123",
        VirtualHost = "/",
    };

    return factory.CreateConnectionAsync().GetAwaiter().GetResult();
});


// PASSO 5: Registrar serviços da aplicação
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddTransient(sp => new CreateCategoryHandler(sp.GetRequiredService<ICategoryRepository>()));
builder.Services.AddTransient(sp => new CreateProductHandler(sp.GetRequiredService<IProductRepository>()));
builder.Services.AddTransient(sp => new GetCategoryAllHandler(sp.GetRequiredService<ICategoryRepository>()));
builder.Services.AddTransient(sp => new GetCategoryByIdHandler(sp.GetRequiredService<ICategoryRepository>()));
builder.Services.AddTransient(sp => new GetProductAllHandler(sp.GetRequiredService<IProductRepository>()));
builder.Services.AddTransient(sp => new GetProductByIdHandler(sp.GetRequiredService<IProductRepository>()));
builder.Services.AddTransient(sp => new GetProductsByCategoryHandler(sp.GetRequiredService<IProductRepository>()));
builder.Services.AddTransient(sp => new UpdateCategoryHandler(sp.GetRequiredService<ICategoryRepository>()));
builder.Services.AddTransient(sp => new UpdateProductHandler(sp.GetRequiredService<IProductRepository>()));
builder.Services.AddTransient(sp => new UpdateStockHandler(sp.GetRequiredService<IProductRepository>()));


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

// PASSO 8: Construir a aplicação
var app = builder.Build();

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
