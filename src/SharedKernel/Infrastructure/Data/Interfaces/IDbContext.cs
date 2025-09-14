using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SharedKernel.Infrastructure.Data.Interfaces;

public interface IDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    EntityEntry Entry(object entity);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}