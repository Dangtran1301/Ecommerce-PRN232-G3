using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.API.Services.Interfaces
{
    public interface IStockService
    {
        Task<Result<StockDto>> GetByIdAsync(Guid id);
        Task<Result<IReadOnlyList<StockDto>>> GetAllAsync();
        Task<Result> CreateAsync(CreateStockRequest request);
        Task<Result> UpdateAsync(Guid id, UpdateStockRequest request);
        Task<Result> DeleteAsync(Guid id);
        //Task<Result<IReadOnlyList<StockDto>>> FilterBySpecification(StockFilterDto filter);
        Task<Result<PagedResult<StockDto>>> FilterByDynamic(DynamicQuery query);
        Task<Result<PagedResult<StockDto>>> FilterPaged(PagedRequest request);
    }
}
