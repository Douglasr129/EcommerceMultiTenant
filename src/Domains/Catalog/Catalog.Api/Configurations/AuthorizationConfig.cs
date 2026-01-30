using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Catalog.Api.Configurations
{
    public static class AuthorizationConfig
    {
        public static IServiceCollection AddAuthorizationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSecret = configuration["Jwt:SecretKey"];
            var validAudience = configuration["Jwt:Audience"];
            var validIssuer = configuration["Jwt:Issuer"];

            services.AddAuthorization(options =>
            {
                // Define as policies com nomes claros
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
                options.AddPolicy("SellerPolicy", policy => policy.RequireRole("Seller"));
                options.AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer"));

                // Policy genérica para qualquer usuário autenticado
                options.AddPolicy("AuthenticatedPolicy", policy => policy.RequireAuthenticatedUser());
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = validAudience,
                    ValidIssuer = validIssuer,
                };
            });
            return services;
        }
    }
}
