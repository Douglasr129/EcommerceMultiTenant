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
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("ADMIN"));
                options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("MANAGER"));
                options.AddPolicy("SellerPolicy", policy => policy.RequireRole("SELLER"));
                options.AddPolicy("ClientPolicy", policy => policy.RequireRole("CLIENT"));
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
