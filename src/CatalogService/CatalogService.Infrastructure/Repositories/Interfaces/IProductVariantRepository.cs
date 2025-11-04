using CatalogService.Entities;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.Infrastructure.Repositories.Interfaces
{
    public interface IProductVariantRepository : IRepository<ProductVariant, Guid>
    {
        Task<IReadOnlyList<ProductVariant>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
        IQueryable<ProductVariant> GetQueryable();
    }
}
