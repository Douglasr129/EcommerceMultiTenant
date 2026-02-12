# Fase base (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Fase de Compilação (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# CORREÇÃO DOS CAMINHOS: Adicionando o prefixo da sua estrutura de pastas
COPY ["src/Domains/Catalog/Catalog.Api/Catalog.Api.csproj", "src/Domains/Catalog/Catalog.Api/"]
COPY ["src/Domains/Catalog/Catalog.Application/Catalog.Application.csproj", "src/Domains/Catalog/Catalog.Application/"]
COPY ["src/Domains/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "src/Domains/Catalog/Catalog.Infrastructure/"]
COPY ["src/Domains/Catalog/Catalog.Domain/Catalog.Domain.csproj", "src/Domains/Catalog/Catalog.Domain/"]

# Restaura as dependências
RUN dotnet restore "src/Domains/Catalog/Catalog.Api/Catalog.Api.csproj"

# Copia o restante do código
COPY . .

# Move para a pasta do projeto da API para compilar
WORKDIR "/src/src/Domains/Catalog/Catalog.Api"
RUN dotnet build "Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Fase de Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Fase Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]