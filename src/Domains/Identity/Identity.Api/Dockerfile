# Fase base (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Fase de Compilação (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# CORREÇÃO DOS CAMINHOS: Ajustado para refletir src/Domains/Identity/
COPY ["src/Domains/Identity/Identity.Api/Identity.Api.csproj", "src/Domains/Identity/Identity.Api/"]
COPY ["src/Domains/Identity/Identity.Application/Identity.Application.csproj", "src/Domains/Identity/Identity.Application/"]
COPY ["src/Domains/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "src/Domains/Identity/Identity.Infrastructure/"]
COPY ["src/Domains/Identity/Identity.Domain/Identity.Domain.csproj", "src/Domains/Identity/Identity.Domain/"]

# Restaura as dependências usando o caminho correto
RUN dotnet restore "src/Domains/Identity/Identity.Api/Identity.Api.csproj"

# Copia todo o código da raiz (EcommerceMultiTenant) para o container
COPY . .

# Move para a pasta do projeto da API dentro da estrutura de pastas do container
WORKDIR "/src/src/Domains/Identity/Identity.Api"
RUN dotnet build "Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Fase de Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Fase Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]