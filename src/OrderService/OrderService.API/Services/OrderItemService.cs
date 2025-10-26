using AutoMapper;
using OrderService.API.DTOs;
using OrderService.API.Errors;
using OrderService.API.Models;
using OrderService.API.Repositories.Interfaces;
using OrderService.API.Services.Interfaces;
using SharedKernel.Domain.Common.Results;

namespace OrderService.API.Services
{
    public class OrderItemService(
        IOrderItemRepository repository,
        IMapper mapper
    ) : IOrderItemService
    {
        public async Task<Result<IReadOnlyList<OrderItemDto>>> GetByOrderIdAsync(Guid orderId)
        {
            var items = await repository.GetByOrderIdAsync(orderId);
            if (items == null || items.Count == 0)
                return OrderErrors.NotFound(orderId);

            var dto = mapper.Map<IReadOnlyList<OrderItemDto>>(items);
            return Result.Ok(dto);
        }

        public async Task<Result> CreateAsync(CreateOrderItemRequest request)
        {
            if (request == null || request.Quantity <= 0)
                return OrderErrors.InvalidData("Order item data is invalid");

            var entity = mapper.Map<OrderItem>(request);
            await repository.AddAsync(entity);
            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateOrderItemRequest request)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
                return OrderErrors.NotFound(id);

            mapper.Map(request, entity);
            await repository.Update(entity);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
                return OrderErrors.NotFound(id);

            await repository.Remove(entity);
            return Result.Ok();
        }
    }
}
