using CatalogService.Entities;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.Infrastructure.Repositories.Interfaces
{
    public interface IProductAttributeRepository : IRepository<ProductAttribute, Guid>
    {
        Task<IReadOnlyList<ProductAttribute>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
        IQueryable<ProductAttribute> GetQueryable();
    }
}