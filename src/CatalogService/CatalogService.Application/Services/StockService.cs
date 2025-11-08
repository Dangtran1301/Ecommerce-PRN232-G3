using AutoMapper;
using CatalogService.Application.DTOs.Stocks;
using CatalogService.Application.Errors;
using CatalogService.Application.Services.Interfaces;
using CatalogService.Entities;
using CatalogService.Infrastructure.Repositories.Interfaces;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.Application.Services
{
    public class StockService(
        IStockRepository stockRepository,
        ISpecificationRepository<Stock> specificationRepository,
        IDynamicRepository<Stock> dynamicRepository,
        IMapper mapper
    ) : IStockService
    {
        public async Task<Result<StockDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var stock = await stockRepository.GetByIdAsync(id, cancellationToken);
            return stock is null
                ? StockErrors.NotFound(id)
                : mapper.Map<StockDto>(stock);
        }

        public async Task<Result<IReadOnlyList<StockDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var stocks = await stockRepository.GetAllAsync(cancellationToken);
            return Result.Ok(mapper.Map<IReadOnlyList<StockDto>>(stocks));
        }

        public async Task<Result> CreateAsync(CreateStockRequest request, CancellationToken cancellationToken = default)
        {
            request.Location = request.Location?.Trim();

            if (await stockRepository.AnyAsync(s => s.ProductId == request.ProductId, cancellationToken))
                return StockErrors.DuplicateProduct(request.ProductId);

            var stock = mapper.Map<Stock>(request);
            await stockRepository.AddAsync(stock, cancellationToken);
            return true;
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateStockRequest request, CancellationToken cancellationToken = default)
        {
            var stock = await stockRepository.GetByIdAsync(id, cancellationToken);
            if (stock is null)
                return StockErrors.NotFound(id);

            request.Location = request.Location?.Trim();
            mapper.Map(request, stock);
            await stockRepository.Update(stock, cancellationToken);
            return true;
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var stock = await stockRepository.GetByIdAsync(id, cancellationToken);
            if (stock is null)
                return StockErrors.NotFound(id);

            await stockRepository.Remove(stock, cancellationToken);
            return true;
        }

        public async Task<Result<IReadOnlyList<StockDto>>> FilterBySpecification(StockFilterDto filter, CancellationToken cancellationToken = default)
        {
            var spec = new StockFilterSpecification(filter);
            var stocks = await specificationRepository.ListAsync(spec, cancellationToken);
            return Result.Ok(mapper.Map<IReadOnlyList<StockDto>>(stocks));
        }

        public async Task<Result<PagedResult<StockDto>>> FilterPaged(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await stockRepository.GetPagedAsync(request, cancellationToken);
            var dto = result.Map(mapper.Map<IReadOnlyList<StockDto>>(result.Items));
            return Result.Ok(dto);
        }

        public IQueryable<StockDto> AsQueryable()
        {
            return stockRepository.GetQueryable().Select(s => new StockDto
            {
                Id = s.Id,
                ProductId = s.ProductId,
                Quantity = s.Quantity,
                Location = s.Location
            });
        }
    }
}