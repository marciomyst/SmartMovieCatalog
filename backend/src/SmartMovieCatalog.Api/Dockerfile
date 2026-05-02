# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled-extra AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to publish the service project.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 \
    NUGET_XMLDOC_MODE=skip \
    NODE_OPTIONS=--max-old-space-size=1024 \
    NPM_CONFIG_AUDIT=false \
    NPM_CONFIG_FUND=false \
    NPM_CONFIG_PROGRESS=false

RUN apt-get update \
    && apt-get install -y --no-install-recommends ca-certificates curl \
    && curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y --no-install-recommends nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.csproj", "backend/src/SmartMovieCatalog.Api/"]
COPY ["backend/src/SmartMovieCatalog.Application/SmartMovieCatalog.Application.csproj", "backend/src/SmartMovieCatalog.Application/"]
COPY ["backend/src/SmartMovieCatalog.Contracts/SmartMovieCatalog.Contracts.csproj", "backend/src/SmartMovieCatalog.Contracts/"]
COPY ["backend/src/SmartMovieCatalog.Domain/SmartMovieCatalog.Domain.csproj", "backend/src/SmartMovieCatalog.Domain/"]
COPY ["backend/src/SmartMovieCatalog.Infrastructure/SmartMovieCatalog.Infrastructure.csproj", "backend/src/SmartMovieCatalog.Infrastructure/"]
COPY ["frontend/Smartmoviecatalog.Angular.esproj", "frontend/"]
COPY ["frontend/package.json", "frontend/"]
COPY ["frontend/package-lock.json", "frontend/"]

RUN dotnet restore "./backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.csproj" --disable-parallel
COPY . .

WORKDIR "/src/backend/src/SmartMovieCatalog.Api"
RUN dotnet publish "./SmartMovieCatalog.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore \
    --no-self-contained \
    /p:UseAppHost=false \
    /p:BuildInParallel=false \
    -maxcpucount:1

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 CMD ["dotnet", "SmartMovieCatalog.Api.dll", "--healthcheck"]
ENTRYPOINT ["dotnet", "SmartMovieCatalog.Api.dll"]
