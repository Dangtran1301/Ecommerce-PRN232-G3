using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.Common.Entities.Interface;

namespace SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

public interface ISpecificationRepository<TEntity, in TKey>
where TEntity : IEntity<TKey>
{
    Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

    Task<TEntity?> GetEntityAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

    Task<int> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);
}