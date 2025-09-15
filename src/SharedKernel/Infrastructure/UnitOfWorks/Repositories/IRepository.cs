using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Entities.Interface;

namespace SharedKernel.Infrastructure.UnitOfWorks.Repositories;

public interface IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    void Update(TEntity entity);

    void Remove(TEntity entity);

    Task<PagedResult<TEntity>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
}