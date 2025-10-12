using CatalogService.API.Repositories.Interfaces;
using CatalogService.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace CatalogService.API.Repositories
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

        public async Task<bool> HasEnoughStockAsync(Guid productId, int requiredQuantity, CancellationToken cancellationToken = default)
        {
            var stock = await _stocks.FirstOrDefaultAsync(s => s.ProductId == productId, cancellationToken);
            return stock != null && stock.Quantity >= requiredQuantity;
        }
    }
}
