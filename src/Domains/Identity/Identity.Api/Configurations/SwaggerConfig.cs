using Microsoft.OpenApi;

namespace Identity.Api.Configurations
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                // Informações da API
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Auth API",
                    Version = "v1",
                    Description = "API de autenticação, criação e manutenção de usuários - E-commerce Multi-Tenant",
                    Contact = new OpenApiContact
                    {
                        Name = "Douglas T. Rodrigues",
                        Email = "Douglas.r129@outlook.com"
                    }
                });

                // Configuração de autenticação Bearer JWT
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT no formato: Bearer {seu token}"
                });

                options.AddSecurityRequirement(docment => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", docment)] = [],
                });

                // Habilitar comentários XML (opcional)
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API v1");
                options.RoutePrefix = "swagger";
                options.DocumentTitle = "Catalog API - Documentação";

                // Configurações adicionais de UI
                options.DisplayRequestDuration();
                options.EnableDeepLinking();
                options.EnableFilter();
                options.ShowExtensions();

            });

            return app;
        }
    }
}
