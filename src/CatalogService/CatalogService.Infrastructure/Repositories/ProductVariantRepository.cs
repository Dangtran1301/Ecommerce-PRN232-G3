using CatalogService.Entities;
using CatalogService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace CatalogService.Infrastructure.Repositories
{
    public class ProductVariantRepository(IDbContext dbContext)
        : EfRepository<ProductVariant, Guid>(dbContext), IProductVariantRepository
    {
        private readonly DbSet<ProductVariant> _variants = dbContext.Set<ProductVariant>();

        public async Task<IReadOnlyList<ProductVariant>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _variants
                .Where(v => v.ProductId == productId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public IQueryable<ProductVariant> GetQueryable()
        {
            return _variants.AsNoTracking();
        }
    }
}