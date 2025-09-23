using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Common;
using SharedKernel.Application.Extensions;
using SharedKernel.Domain.Common.Entities;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace SharedKernel.Infrastructure.UnitOfWorks.Repositories;

public class DynamicRepository<TEntity, TKey>(IDbContext dbContext) : EfRepository<TEntity, TKey>(dbContext), IDynamicRepository<TEntity>
    where TEntity : Entity<TKey>
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task<PagedResult<TEntity>> GetPagedAsync(DynamicQuery query, CancellationToken cancellationToken = default)
    {
        var q = _dbSet.AsNoTracking();

        if (query.Filters?.Any() == true)
            q = q.ApplyFilters(query.Filters);

        q = q.ApplySorting(query.SortBy, query.Descending);

        return await q.ToPagedResultAsync(query, cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> ListAsync(DynamicQuery query, CancellationToken cancellationToken = default)
    {
        var q = _dbSet.AsNoTracking();

        if (query.Filters?.Any() == true)
            q = q.ApplyFilters(query.Filters);

        q = q.ApplySorting(query.SortBy, query.Descending);

        return await q.ToListAsync(cancellationToken);
    }
}