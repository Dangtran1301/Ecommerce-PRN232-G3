using CatalogService.Entities;
using CatalogService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace CatalogService.Infrastructure.Repositories;

public class ProductAttributeRepository(IDbContext dbContext)
    : EfRepository<ProductAttribute, Guid>(dbContext), IProductAttributeRepository
{
    private readonly DbSet<ProductAttribute> _attributes = dbContext.Set<ProductAttribute>();

    public async Task<IReadOnlyList<ProductAttribute>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _attributes
            .Where(x => x.ProductId == productId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public IQueryable<ProductAttribute> GetQueryable()
    {
        return _attributes.AsNoTracking();
    }
}
