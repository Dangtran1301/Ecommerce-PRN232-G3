FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/CatalogService/CatalogService.API/CatalogService.API.csproj", "src/CatalogService/CatalogService.API/"]
COPY ["src/CatalogService/CatalogService.Application/CatalogService.Application.csproj", "src/CatalogService/CatalogService.Application/"]
COPY ["src/CatalogService/CatalogService.Domain/CatalogService.Domain.csproj", "src/CatalogService/CatalogService.Domain/"]
COPY ["src/CatalogService/CatalogService.Infrastructure/CatalogService.Infrastructure.csproj", "src/CatalogService/CatalogService.Infrastructure/"]
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]

RUN dotnet restore "src/CatalogService/CatalogService.API/CatalogService.API.csproj"
COPY . .
WORKDIR "/src/src/CatalogService/CatalogService.API"
RUN dotnet publish "CatalogService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CatalogService.API.dll"]
