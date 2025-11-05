using CatalogService.Entities;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.API.Repositories.Interfaces
{
    public interface IProductVariantRepository : IRepository<ProductVariant, Guid>
    {
        Task<IReadOnlyList<ProductVariant>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

        Task<ProductVariant?> GetByNameAsync(Guid productId, string variantName, CancellationToken cancellationToken = default);
    }
}