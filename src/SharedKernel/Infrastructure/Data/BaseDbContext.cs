using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Common.Entities.Interface;
using SharedKernel.Infrastructure.Data.Interfaces;

namespace SharedKernel.Infrastructure.Data;

public class BaseDbContext : DbContext, IDbContext
{
    protected BaseDbContext(DbContextOptions options) : base(options) { }

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
            entry.State = EntityState.Modified;
            entry.Entity.MarkAsDeleted("system");
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}