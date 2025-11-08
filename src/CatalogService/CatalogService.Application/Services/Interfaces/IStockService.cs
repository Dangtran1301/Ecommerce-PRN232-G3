using CatalogService.Application.DTOs.Stocks;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.Application.Services.Interfaces
{
    public interface IStockService
    {
        Task<Result<StockDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Result<IReadOnlyList<StockDto>>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Result> CreateAsync(CreateStockRequest request, CancellationToken cancellationToken = default);

        Task<Result> UpdateAsync(Guid id, UpdateStockRequest request, CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Result<IReadOnlyList<StockDto>>> FilterBySpecification(StockFilterDto filter, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<StockDto>>> FilterPaged(PagedRequest request, CancellationToken cancellationToken = default);

        IQueryable<StockDto> AsQueryable();
    }
}