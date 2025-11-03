# -------------------------------
# Base runtime image
# -------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# -------------------------------
# Build & publish stage
# -------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy từng .csproj để tận dụng cache cho restore
COPY ["src/UserService/UserService.API/UserService.API.csproj", "src/UserService/UserService.API/"]
COPY ["src/UserService/UserService.Application/UserService.Application.csproj", "src/UserService/UserService.Application/"]
COPY ["src/UserService/UserService.Domain/UserService.Domain.csproj", "src/UserService/UserService.Domain/"]
COPY ["src/UserService/UserService.Infrastructure/UserService.Infrastructure.csproj", "src/UserService/UserService.Infrastructure/"]
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]

# Restore dependencies
RUN dotnet restore "src/UserService/UserService.API/UserService.API.csproj"

# Copy toàn bộ source
COPY . .

# Build & publish trong 1 bước (nhanh hơn, nhẹ hơn)
WORKDIR "/src/src/UserService/UserService.API"
RUN dotnet publish "UserService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# -------------------------------
# Final runtime image
# -------------------------------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UserService.API.dll"]