using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Extensions;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.Common.Entities;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace SharedKernel.Infrastructure.UnitOfWorks.Repositories;

public class SpecificationRepository<TEntity, TKey>(IDbContext dbContext) : EfRepository<TEntity, TKey>(dbContext), ISpecificationRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        => await SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), spec).AsNoTracking().ToListAsync(cancellationToken);

    public async Task<TEntity?> GetEntityAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        => await SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), spec).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

    public async Task<int> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        => await SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), spec).CountAsync(cancellationToken);
}