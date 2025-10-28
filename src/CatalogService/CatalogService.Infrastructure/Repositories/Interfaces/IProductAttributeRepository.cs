using CatalogService.Entities;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.API.Repositories.Interfaces
{
    public interface IProductAttributeRepository : IRepository<ProductAttribute, Guid>
    {
        Task<IReadOnlyList<ProductAttribute>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<ProductAttribute?> GetByNameAsync(Guid productId, string attributeName, CancellationToken cancellationToken = default);
    }
}
