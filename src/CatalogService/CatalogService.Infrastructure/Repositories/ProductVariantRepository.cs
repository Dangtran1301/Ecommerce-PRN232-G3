using CatalogService.API.Repositories.Interfaces;
using CatalogService.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace CatalogService.API.Repositories
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

        public async Task<ProductVariant?> GetByNameAsync(Guid productId, string variantName, CancellationToken cancellationToken = default)
        {
            return await _variants
                .FirstOrDefaultAsync(v => v.ProductId == productId && v.VariantName == variantName, cancellationToken);
        }
    }
}
