using CatalogService.Entities;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.API.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product, Guid>
    {
        Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Product>> GetByBrandIdAsync(Guid brandId, CancellationToken cancellationToken = default);

        // 👇 THÊM 2 HÀM NÀY
        Task<IReadOnlyList<Product>> GetAllWithRelationsAsync(CancellationToken cancellationToken = default);

        Task<Product?> GetByIdWithRelationsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}