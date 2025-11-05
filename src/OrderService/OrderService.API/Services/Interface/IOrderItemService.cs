using OrderService.API.DTOs;
using SharedKernel.Domain.Common.Results;

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