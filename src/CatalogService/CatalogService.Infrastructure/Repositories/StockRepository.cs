using CatalogService.Entities;
using CatalogService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace CatalogService.Infrastructure.Repositories
{
    public class StockRepository(IDbContext dbContext)
        : EfRepository<Stock, Guid>(dbContext), IStockRepository
    {
        private readonly DbSet<Stock> _stocks = dbContext.Set<Stock>();

        public async Task<Stock?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _stocks
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ProductId == productId, cancellationToken);
        }

        public IQueryable<Stock> GetQueryable()
        {
            return _stocks
                .Include(s => s.Product)
                .AsNoTracking();
        }
    }
}