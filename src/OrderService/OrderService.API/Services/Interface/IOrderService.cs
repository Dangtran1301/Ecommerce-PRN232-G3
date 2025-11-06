using OrderService.API.DTOs;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;

namespace OrderService.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Result<OrderDto>> GetByIdAsync(Guid id);

        Task<Result<IReadOnlyList<OrderDto>>> GetAllAsync();

        Task<Result> CreateAsync(CreateOrderRequest request);

        Task<Result> UpdateAsync(Guid id, UpdateOrderRequest request);

        Task<Result> DeleteAsync(Guid id);

        Task<Result<IReadOnlyList<OrderDto>>> GetByCustomerIdAsync(Guid customerId);

        Task<Result<OrderDto>> GetOrderWithItemsAsync(Guid orderId);

        Task<Result<PagedResult<OrderDto>>> FilterPaged(PagedRequest request);

        Task<Result<PagedResult<OrderDto>>> GetAllPagedAsync(int pageIndex, int pageSize);
    }
}