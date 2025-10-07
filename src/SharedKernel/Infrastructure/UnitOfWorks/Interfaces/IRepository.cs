using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Entities.Interface;
using System.Linq.Expressions;

namespace SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

public interface IRepository<TEntity, in TKey> where TEntity : IEntity<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task Update(TEntity entity, CancellationToken cancellationToken = default);

    Task Remove(TEntity entity, CancellationToken cancellationToken = default);

    Task<PagedResult<TEntity>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
}