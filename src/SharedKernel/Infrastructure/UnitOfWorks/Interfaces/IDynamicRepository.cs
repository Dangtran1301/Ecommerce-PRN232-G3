using SharedKernel.Application.Common;

namespace SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

public interface IDynamicRepository<TEntity>
{
    Task<PagedResult<TEntity>> GetPagedAsync(DynamicQuery query, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> ListAsync(DynamicQuery query, CancellationToken cancellationToken = default);
}