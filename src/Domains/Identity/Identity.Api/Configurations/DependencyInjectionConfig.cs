using Identity.Application.Handlers;
using Identity.Domain.Interfaces;
using Identity.Infrastructure.Messaging;
using Identity.Infrastructure.Repisitories;
using Identity.Infrastructure.Repositories;
using Identity.Infrastructure.Security;

namespace Identity.Api.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, string jwtSecretKey)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<UserCreatedPublisher>();
            // Handlers
            services.AddScoped<DeleteUserHandler>();
            services.AddScoped<GetAllUserHandler>();
            services.AddScoped<GetUserHandler>();
            services.AddScoped<LoginUserHandler>();
            services.AddScoped<RegisterUserHandler>();
            services.AddScoped<UpdateRoleHandler>();
            services.AddScoped<UpdateUserHandler>();
            services.AddScoped<ITokenService>(provider =>
                new TokenService(jwtSecretKey));

            return services;
        }
    }
}
