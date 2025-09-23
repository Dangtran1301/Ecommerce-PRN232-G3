using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Common.Entities.Interface;
using SharedKernel.Infrastructure.Data.Interfaces;
using System.Linq.Expressions;

namespace SharedKernel.Infrastructure.Data;

public class BaseDbContext : DbContext, IDbContext
{
    protected BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplySoftDeleteFilter();
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<IAuditable>();
        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.SetCreated("system");

            if (entry.State == EntityState.Modified)
                entry.Entity.SetUpdated("system");
        }

        var deletedEntries = ChangeTracker.Entries<ISoftDelete>()
            .Where(e => e.State == EntityState.Deleted);

        foreach (var entry in deletedEntries)
        {
            entry.State = EntityState.Unchanged;
            entry.Entity.MarkAsDeleted("system");
            Entry(entry.Entity).Property(e => e.IsDeleted).IsModified = true;
            Entry(entry.Entity).Property(e => e.DeletedAt).IsModified = true;
            Entry(entry.Entity).Property(e => e.DeletedBy).IsModified = true;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}

public static class ModelBuilderExtensions
{
    public static void ApplySoftDeleteFilter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType)) continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var prop = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
            var filter = Expression.Lambda(
                Expression.Equal(prop, Expression.Constant(false)),
                parameter
            );

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
        }
    }
}