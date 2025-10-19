using AutoMapper;
using OrderService.API.DTOs;
using OrderService.API.Errors;
using OrderService.API.Models;
using OrderService.API.Repositories.Interfaces;
using OrderService.API.Services.Interfaces;
using OrderService.API.Specifications;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace OrderService.API.Services
{
    public class OrderService(
        IOrderRepository repository,
        ISpecificationRepository<Order> specificationRepository,
        IDynamicRepository<Order> dynamicRepository,
        IMapper mapper
    ) : IOrderService
    {
        public async Task<Result<OrderDto>> GetByIdAsync(Guid id)
        {
            var order = await repository.GetByIdAsync(id);
            return order is null
                ? OrderErrors.NotFound(id)
                : mapper.Map<OrderDto>(order);
        }

        public async Task<Result<IReadOnlyList<OrderDto>>> GetAllAsync()
        {
            var orders = await repository.GetAllAsync();
            return Result.Success(mapper.Map<IReadOnlyList<OrderDto>>(orders));
        }

        public async Task<Result> CreateAsync(CreateOrderRequest request)
        {
            var entity = mapper.Map<Order>(request);
            await repository.AddAsync(entity);
            return Result.Success();
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateOrderRequest request)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null)
                return OrderErrors.NotFound(id);

            mapper.Map(request, entity);
            await repository.Update(entity);
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null)
                return OrderErrors.NotFound(id);

            await repository.Remove(entity);
            return Result.Success();
        }

        public async Task<Result<IReadOnlyList<OrderDto>>> GetByCustomerIdAsync(Guid customerId)
        {
            var orders = await repository.GetOrdersByCustomerIdAsync(customerId);
            return Result.Success(mapper.Map<IReadOnlyList<OrderDto>>(orders));
        }

        public async Task<Result<OrderDto>> GetOrderWithItemsAsync(Guid orderId)
        {
            var order = await repository.GetOrderWithItemsAsync(orderId);
            return order is null
                ? OrderErrors.NotFound(orderId)
                : mapper.Map<OrderDto>(order);
        }

        public async Task<Result<PagedResult<OrderDto>>> FilterPaged(PagedRequest request)
        {
            var result = await repository.GetPagedAsync(request);
            var dto = result.Map(mapper.Map<IReadOnlyList<OrderDto>>(result.Items));
            return Result.Success(dto);
        }

        public async Task<Result<PagedResult<OrderDto>>> GetAllPagedAsync(int pageIndex, int pageSize)
        {
            var request = new PagedRequest
            {
                PageNumber = pageIndex,
                PageSize = pageSize
            };

            var result = await repository.GetPagedAsync(request);
            var dto = result.Map(mapper.Map<IReadOnlyList<OrderDto>>(result.Items));
            return Result.Success(dto);
        }

        public async Task<Result<PagedResult<OrderDto>>> FilterByDynamic(DynamicQuery query)
        {
            var result = await dynamicRepository.GetPagedAsync(query);
            var dto = result.Map(mapper.Map<IReadOnlyList<OrderDto>>(result.Items));
            return Result.Success(dto);
        }

        public async Task<Result<IReadOnlyList<OrderDto>>> FilterBySpecification(OrderFilterDto filter)
        {
            var spec = new OrderFilterSpecification(filter);
            var list = await specificationRepository.ListAsync(spec);
            return Result.Success(mapper.Map<IReadOnlyList<OrderDto>>(list));
        }
    }
}
