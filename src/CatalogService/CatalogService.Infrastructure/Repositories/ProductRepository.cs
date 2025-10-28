using CatalogService.API.Repositories.Interfaces;
using CatalogService.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace CatalogService.API.Repositories
{
    public class ProductRepository(IDbContext dbContext)
        : EfRepository<Product, Guid>(dbContext), IProductRepository
    {
        private readonly DbSet<Product> _products = dbContext.Set<Product>();


        //Add temp
        public async Task<IReadOnlyList<Product>> GetAllWithRelationsAsync(CancellationToken cancellationToken = default)
        {
            return await _products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Stock)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdWithRelationsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Stock)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }











        public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductName == name, cancellationToken);
        }

        public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
        {
            return await _products
                .FirstOrDefaultAsync(p => p.Sku == sku, cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
        {
            return await _products
                .Where(p => p.CategoryId == categoryId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> GetByBrandIdAsync(Guid brandId, CancellationToken cancellationToken = default)
        {
            return await _products
                .Where(p => p.BrandId == brandId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
