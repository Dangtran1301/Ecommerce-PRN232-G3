using OrderService.API.Models;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace OrderService.API.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        Task<Order?> GetOrderWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Order>> GetOrdersByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    }
}
