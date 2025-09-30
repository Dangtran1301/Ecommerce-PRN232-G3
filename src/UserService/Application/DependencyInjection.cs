using Microsoft.Extensions.DependencyInjection;
using UserService.Application.DTOs;
using UserService.Application.Services.Interfaces;

namespace UserService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(UserProfile).Assembly);

        // Application Services
        services.AddScoped<IUserService, Services.UserService>();

        return services;
    }
}