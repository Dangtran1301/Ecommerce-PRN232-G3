using OrderService.API.Models;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace OrderService.API.Repositories.Interfaces
{
    public interface IOrderItemRepository : IRepository<OrderItem, Guid>
    {
        Task<IReadOnlyList<OrderItem>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    }
}