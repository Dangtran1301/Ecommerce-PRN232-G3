using CatalogService.Entities;
using CatalogService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace CatalogService.Infrastructure.Repositories
{
    public class ProductRepository(IDbContext dbContext)
        : EfRepository<Product, Guid>(dbContext), IProductRepository
    {
        private readonly DbSet<Product> _products = dbContext.Set<Product>();

        public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Stock)
                .Include(p => p.Variants)
                .Include(p => p.Attributes)
                .FirstOrDefaultAsync(p => p.ProductName == name, cancellationToken);
        }

        public IQueryable<Product> GetQueryable()
        {
            return _products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Stock)
                .Include(p => p.Variants)
                .Include(p => p.Attributes)
                .AsNoTracking();
        }
    }
}
