using SharedKernel.Domain.Common.Results;
using OrderService.API.DTOs;

namespace OrderService.API.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<Result<IReadOnlyList<OrderItemDto>>> GetByOrderIdAsync(Guid orderId);
        Task<Result> CreateAsync(CreateOrderItemRequest request);
        Task<Result> UpdateAsync(Guid id, UpdateOrderItemRequest request);
        Task<Result> DeleteAsync(Guid id);
    }
}
