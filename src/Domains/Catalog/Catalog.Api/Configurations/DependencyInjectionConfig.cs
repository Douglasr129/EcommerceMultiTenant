using Catalog.Api.Services;
using Catalog.Application.Handlers.CategoryHandlers;
using Catalog.Application.Handlers.ProductHandlers;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Repositories;

namespace Catalog.Api.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // Product Handlers
            services.AddTransient(sp => new CreateProductHandler(sp.GetRequiredService<IProductRepository>()));
            services.AddTransient(sp => new DeleteProductHandler(sp.GetRequiredService<IProductRepository>()));
            services.AddTransient(sp => new GetProductAllHandler(sp.GetRequiredService<IProductRepository>()));
            services.AddTransient(sp => new GetProductByIdHandler(sp.GetRequiredService<IProductRepository>()));
            services.AddTransient(sp => new GetProductsByCategoryHandler(sp.GetRequiredService<IProductRepository>()));
            services.AddTransient(sp => new UpdateProductHandler(sp.GetRequiredService<IProductRepository>()));
            services.AddTransient(sp => new UpdateStockHandler(sp.GetRequiredService<IProductRepository>()));
            services.AddTransient(sp => new UpdateProductsByCategoryHandler(sp.GetRequiredService<IProductRepository>()));

            // Category Handlers
            services.AddTransient(sp => new CreateCategoryHandler(sp.GetRequiredService<ICategoryRepository>()));
            services.AddTransient(sp => new DeleteCategoryHandler(sp.GetRequiredService<ICategoryRepository>()));
            services.AddTransient(sp => new GetCategoryAllHandler(sp.GetRequiredService<ICategoryRepository>()));
            services.AddTransient(sp => new GetCategoryByIdHandler(sp.GetRequiredService<ICategoryRepository>()));
            services.AddTransient(sp => new UpdateCategoryHandler(sp.GetRequiredService<ICategoryRepository>()));

            // Injeção de Serviços
            services.AddHttpContextAccessor();
            services.AddScoped<ILinkService, LinkService>();
            services.AddScoped<IUserContextService, UserContextService>();

            return services;
        }
    }
}
