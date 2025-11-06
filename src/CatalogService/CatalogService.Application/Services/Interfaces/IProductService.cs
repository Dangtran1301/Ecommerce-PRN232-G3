using CatalogService.Application.DTOs.Products;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<Result<ProductDto>> GetByIdAsync(Guid id);
        Task<Result<IReadOnlyList<ProductDto>>> GetAllAsync();
        Task<Result> CreateAsync(CreateProductRequest request);
        Task<Result> UpdateAsync(Guid id, UpdateProductRequest request);
        Task<Result> DeleteAsync(Guid id);
        Task<Result<IReadOnlyList<ProductDto>>> FilterBySpecification(ProductFilterDto filter);
        Task<Result<PagedResult<ProductDto>>> FilterPaged(PagedRequest request);
        IQueryable<ProductDto> AsQueryable();
    }
}
