using AutoMapper;
using CatalogService.API.Errors;
using CatalogService.API.Services.Interfaces;
using CatalogService.Entities;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.API.Services
{
    public class StockService(
        IStockRepository repository,
        ISpecificationRepository<Stock> specificationRepository,
        IDynamicRepository<Stock> dynamicRepository,
        IMapper mapper
    ) : IStockService
    {
        public async Task<Result<StockDto>> GetByIdAsync(Guid id)
        {
            var stock = await repository.GetByIdAsync(id);
            return stock is null
                ? StockErrors.NotFound(id)
                : mapper.Map<StockDto>(stock);
        }

        public async Task<Result<IReadOnlyList<StockDto>>> GetAllAsync()
        {
            var list = await repository.GetAllAsync();
            return Result.Ok(mapper.Map<IReadOnlyList<StockDto>>(list));
        }

        public async Task<Result> CreateAsync(CreateStockRequest request)
        {
            var existing = await repository.GetByProductIdAsync(request.ProductId);
            if (existing is not null)
                return StockErrors.DuplicateProduct(request.ProductId);

            var entity = mapper.Map<Stock>(request);
            await repository.AddAsync(entity);
            return true;
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateStockRequest request)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null)
                return StockErrors.NotFound(id);

            mapper.Map(request, entity);
            await repository.Update(entity);
            return true;
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null)
                return StockErrors.NotFound(id);

            await repository.Remove(entity);
            return true;
        }

        //public async Task<Result<IReadOnlyList<StockDto>>> FilterBySpecification(StockFilterDto filter)
        //{
        //    var spec = new StockFilterSpecification(filter);
        //    var list = await specificationRepository.ListAsync(spec);
        //    return Result.Success(mapper.Map<IReadOnlyList<StockDto>>(list));
        //}

        public async Task<Result<PagedResult<StockDto>>> FilterByDynamic(DynamicQuery query)
        {
            var result = await dynamicRepository.GetPagedAsync(query);
            var dto = result.Map(mapper.Map<IReadOnlyList<StockDto>>(result.Items));
            return Result.Ok(dto);
        }

        public async Task<Result<PagedResult<StockDto>>> FilterPaged(PagedRequest request)
        {
            var result = await repository.GetPagedAsync(request);
            var dto = result.Map(mapper.Map<IReadOnlyList<StockDto>>(result.Items));
            return Result.Ok(dto);
        }
    }
}