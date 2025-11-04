using CatalogService.Application.DTOs.Products;
using CatalogService.Entities;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<ProductDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<ProductDto>> CreateAsync(ProductDto dto, CancellationToken cancellationToken = default);
        Task<Result<ProductDto>> UpdateAsync(ProductDto dto, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PagedResult<ProductDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
        IQueryable<Product> GetQueryable();
    }
}
