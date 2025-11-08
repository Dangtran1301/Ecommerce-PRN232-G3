using AuthService.API.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Common.Entities;

namespace AuthService.API.Data;

public static class AuthDbContextSeed
{
    public static async Task SeedAdminAsync(AuthDbContext context)
    {
        if (!await context.Users.AnyAsync(u => u.Role == Role.Admin))
        {
            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var admin = new User(
                "admin",
                "admin@savonel.com",
                BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role.Admin
            );
            // Use reflection to set the protected Id property
            typeof(Entity<Guid>)
                .GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                ?.SetValue(admin, adminId);

            admin.SetCreated("system");

            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }
}