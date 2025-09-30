using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Data;
using System.Reflection;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options) : BaseDbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserSession> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}