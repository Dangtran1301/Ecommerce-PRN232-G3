using CatalogService.API.Repositories.Interfaces;
using CatalogService.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace CatalogService.API.Repositories
{
    public class ProductAttributeRepository(IDbContext dbContext)
        : EfRepository<ProductAttribute, Guid>(dbContext), IProductAttributeRepository
    {
        private readonly DbSet<ProductAttribute> _attributes = dbContext.Set<ProductAttribute>();

        public async Task<IReadOnlyList<ProductAttribute>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _attributes
                .Where(a => a.ProductId == productId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductAttribute?> GetByNameAsync(Guid productId, string attributeName, CancellationToken cancellationToken = default)
        {
            return await _attributes
                .FirstOrDefaultAsync(a => a.ProductId == productId && a.AttributeName == attributeName, cancellationToken);
        }
    }
}