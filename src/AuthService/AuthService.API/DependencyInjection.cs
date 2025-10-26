using Asp.Versioning;
using AuthService.API.Data;
using AuthService.API.Data.UnitOfWorks;
using AuthService.API.DTOs;
using AuthService.API.Interfaces;
using AuthService.API.Models;
using AuthService.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using System.Text;

namespace AuthService.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiVersioningAndSwagger(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-API-Version"),
                    new UrlSegmentApiVersionReader(),
                    new MediaTypeApiVersionReader("api-version")
                );
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Nhập 'Bearer {token}' vào đây. Ví dụ: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwt = configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwt["Key"]!);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt["Issuer"],
                    ValidAudience = jwt["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));
        });

        return services;
    }

    public static IServiceCollection AddValidationBehavior(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(ms => ms.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var error = Error.Validation(
                    "One or more validation errors occurred",
                    System.Text.Json.JsonSerializer.Serialize(errors)
                );

                var response = new ApiResponse { Error = error };
                return new BadRequestObjectResult(response);
            };
        });

        return services;
    }

    public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services, ConfigurationManager config)
    {
        // DbContext
        services.AddDbContext<AuthDbContext>(opts =>
        {
            opts.UseSqlServer(config.GetConnectionString("AuthDb"));
        });
        services.AddScoped<IDbContext, AuthDbContext>();
        services.AddScoped<AuthDbContext>();

        // AutoMapper
        services.AddAutoMapper(typeof(AuthProfile));

        // Repository
        services.AddScoped<IRepository<RefreshToken, int>, AuthRepository>();

        // Token + Auth Services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService.API.Services.AuthService>();

        // Http Client Internal (User Service)
        services.AddHttpClient("UserServiceClient", client =>
        {
            client.BaseAddress = new Uri(config["InternalApi:BaseUrl"] ?? string.Empty);
            var key = config["InternalApi:Key"];
            if (!string.IsNullOrWhiteSpace(key))
                client.DefaultRequestHeaders.Add("X-Internal-Key", key);
        });
        services.AddScoped<IUserInternalClient, UserInternalClient>();

        // Background worker
        services.AddHostedService<TokenCleanupService>();

        // Misc
        services.AddHttpContextAccessor();

        return services;
    }
}