using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Entities;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace SharedKernel.Infrastructure.UnitOfWorks.Repositories;

public class EfRepository<TEntity, TKey>(IDbContext dbContext) : IRepository<TEntity, TKey>
where TEntity : Entity<TKey>
{
    protected readonly IDbContext DbContext = dbContext;
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default) =>
        await _dbSet.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await _dbSet.AddAsync(entity, cancellationToken);

    public void Update(TEntity entity) => _dbSet.Update(entity);

    public void Remove(TEntity entity) => _dbSet.Remove(entity);

    public async Task<PagedResult<TEntity>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking();

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip(request.Skip)
            .Take(request.Take)
            .ToListAsync(cancellationToken);

        return new PagedResult<TEntity>(items, total, request.PageNumber, request.PageSize);
    }
}