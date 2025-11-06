using SharedKernel.Application.Interfaces;

namespace SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

public interface ISpecificationRepository<TEntity>
{
    Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

    Task<TEntity?> GetEntityAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

    Task<int> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);
}