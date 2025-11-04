using Microsoft.EntityFrameworkCore;
using OrderService.API.Data;
using OrderService.API.Models;
using OrderService.API.Repositories.Interfaces;
using SharedKernel.Infrastructure.UnitOfWorks.Repositories;

namespace OrderService.API.Repositories.Implementations
{
    public class OrderRepository : EfRepository<Order, Guid>, IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepository(OrderDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order?> GetOrderWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.Items)
                .ToListAsync(cancellationToken);
        }
    }
}