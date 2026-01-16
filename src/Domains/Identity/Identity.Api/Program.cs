using EvolveDb;
using Identity.Application.Handlers;
using Identity.Application.Interfaces;
using Identity.Infrastructure.Configurations;
using Identity.Infrastructure.Repisitories;
using Identity.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"]!;

using var cnx = new NpgsqlConnection(connectionString);
cnx.Open(); // Tenta abrir a conexão
Console.WriteLine("? Conexão com o banco estabelecida com sucesso!");
cnx.Close();

var evolve = new Evolve(cnx, msg => Console.WriteLine(msg))
{
    Locations = new[] { "../Identity.Infrastructure/Migrations" },
    IsEraseDisabled = true
};

evolve.Migrate();

builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<RegisterUserHandler>();
builder.Services.AddScoped<LoginUserHandler>();
builder.Services.AddScoped<ITokenService>(provider =>
    new TokenService(jwtSecretKey));






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
