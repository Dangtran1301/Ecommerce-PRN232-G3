using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Entities.Enums;

namespace UserService.Infrastructure.Data;

public static class UserProfileContextSeed
{
    public static async Task SeedAdminAsync(UserDbContext context)
    {
        var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        if (!await context.UserProfiles.AnyAsync(u => u.Id == adminId))
        {
            var profile = new UserProfile(
                adminId,
                "Administrator",
                "0123456789"
            )
            {
                Gender = Gender.Unknown,
            };

            await context.UserProfiles.AddAsync(profile);
            await context.SaveChangesAsync();
        }
    }
}