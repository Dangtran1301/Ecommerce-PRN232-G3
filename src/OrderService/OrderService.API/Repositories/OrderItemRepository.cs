using Microsoft.EntityFrameworkCore;
using OrderService.API.Data;
using OrderService.API.Models;
using OrderService.API.Repositories.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace OrderService.API.Repositories.Implementations
{
    public class OrderItemRepository : EfRepository<OrderItem, Guid>, IOrderItemRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderItemRepository(OrderDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<OrderItem>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.OrderItems
                .Where(item => item.OrderId == orderId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
