FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["CatalogService/CatalogService.API/CatalogService.API.csproj", "CatalogService/CatalogService.API/"]
COPY ["CatalogService/CatalogService.Application/CatalogService.Application.csproj", "CatalogService/CatalogService.Application/"]
COPY ["CatalogService/CatalogService.Domain/CatalogService.Domain.csproj", "CatalogService/CatalogService.Domain/"]
COPY ["CatalogService/CatalogService.Infrastructure/CatalogService.Infrastructure.csproj", "CatalogService/CatalogService.Infrastructure/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]

RUN dotnet restore "CatalogService/CatalogService.API/CatalogService.API.csproj"
COPY . .
WORKDIR "/src/CatalogService/CatalogService.API"
RUN dotnet publish "CatalogService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CatalogService.API.dll"]
