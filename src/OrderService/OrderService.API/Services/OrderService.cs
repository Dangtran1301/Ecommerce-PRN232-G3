using AutoMapper;
using OrderService.API.Clients.Interfaces;
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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly ISpecificationRepository<Order> _specRepository;
        private readonly IDynamicRepository<Order> _dynamicRepository;
        private readonly IMapper _mapper;
        private readonly IProductClient _productClient;

        public OrderService(
            IOrderRepository repository,
            ISpecificationRepository<Order> specRepository,
            IDynamicRepository<Order> dynamicRepository,
            IProductClient productClient,
            IMapper mapper)
        {
            _repository = repository;
            _specRepository = specRepository;
            _dynamicRepository = dynamicRepository;
            _productClient = productClient;
            _mapper = mapper;
        }
        public async Task<Result<OrderDto>> GetByIdAsync(Guid id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order is null)
                return OrderErrors.NotFound(id);

            return Result.Ok(_mapper.Map<OrderDto>(order));
        }

        public async Task<Result<IReadOnlyList<OrderDto>>> GetAllAsync()
        {
            var orders = await _repository.GetAllAsync();
            var dto = _mapper.Map<IReadOnlyList<OrderDto>>(orders);
            return Result.Ok(dto);
        }

        public async Task<Result> CreateAsync(CreateOrderRequest request)
        {
            if (request.Items == null || !request.Items.Any())
                return Error.Validation("Order must have at least one item.");

            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var item in request.Items)
            {
                var product = await _productClient.GetProductByIdAsync(item.ProductId)
              ?? new ProductDto { Id = item.ProductId, ProductName = "Mock", Price = item.Price };

                var price = product.Price;
                totalAmount += price * item.Quantity;

                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = price
                });
            }

            var entity = new Order
            {
                CustomerId = request.CustomerId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = OrderStatus.Pending,
                Items = orderItems
            };

            await _repository.AddAsync(entity);
            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateOrderRequest request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is null)
                return OrderErrors.NotFound(id);

            _mapper.Map(request, entity);
            await _repository.Update(entity);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity is null)
                return OrderErrors.NotFound(id);

            await _repository.Remove(entity);
            return Result.Ok();
        }

        public async Task<Result<IReadOnlyList<OrderDto>>> GetByCustomerIdAsync(Guid customerId)
        {
            var orders = await _repository.GetOrdersByCustomerIdAsync(customerId);
            var dto = _mapper.Map<IReadOnlyList<OrderDto>>(orders);
            return Result.Ok(dto);
        }

        public async Task<Result<OrderDto>> GetOrderWithItemsAsync(Guid orderId)
        {
            var order = await _repository.GetOrderWithItemsAsync(orderId);
            if (order is null)
                return OrderErrors.NotFound(orderId);

            return Result.Ok(_mapper.Map<OrderDto>(order));
        }

        public async Task<Result<PagedResult<OrderDto>>> GetAllPagedAsync(int pageIndex, int pageSize)
        {
            var request = new PagedRequest { PageNumber = pageIndex, PageSize = pageSize };
            return await GetPagedResultAsync(request);
        }

        public async Task<Result<PagedResult<OrderDto>>> FilterPaged(PagedRequest request)
            => await GetPagedResultAsync(request);

        public async Task<Result<PagedResult<OrderDto>>> FilterByDynamic(DynamicQuery query)
        {
            var result = await _dynamicRepository.GetPagedAsync(query);
            var dto = result.Map(_mapper.Map<IReadOnlyList<OrderDto>>(result.Items));
            return Result.Ok(dto);
        }

        public async Task<Result<IReadOnlyList<OrderDto>>> FilterBySpecification(OrderFilterDto filter)
        {
            var spec = new OrderFilterSpecification(filter);
            var list = await _specRepository.ListAsync(spec);
            var dto = _mapper.Map<IReadOnlyList<OrderDto>>(list);
            return Result.Ok(dto);
        }

        private async Task<Result<PagedResult<OrderDto>>> GetPagedResultAsync(PagedRequest request)
        {
            var result = await _repository.GetPagedAsync(request);
            var dto = result.Map(_mapper.Map<IReadOnlyList<OrderDto>>(result.Items));
            return Result.Ok(dto);
        }
    }
}