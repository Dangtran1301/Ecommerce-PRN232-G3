using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data;
using UserService.API.Entities;

namespace UserService.API.Data;

public class UserDbContext : BaseDbContext
{
    public DbSet<User> Users { get; set; }

    protected UserDbContext(DbContextOptions options) : base(options)
    {
    }
}