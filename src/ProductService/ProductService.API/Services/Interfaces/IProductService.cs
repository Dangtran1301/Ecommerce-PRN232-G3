using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using CatalogService.API.DTOs;

namespace CatalogService.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<Result<ProductDto>> GetByIdAsync(Guid id);

        Task<Result<IReadOnlyList<ProductDto>>> GetAllAsync();

        Task<Result> CreateAsync(CreateProductRequest request);

        Task<Result> UpdateAsync(Guid id, UpdateProductRequest request);

        Task<Result> DeleteAsync(Guid id);

        Task<Result<IReadOnlyList<ProductDto>>> FilterBySpecification(ProductFilterDto filter);

        Task<Result<PagedResult<ProductDto>>> FilterByDynamic(DynamicQuery query);

        Task<Result<PagedResult<ProductDto>>> FilterPaged(PagedRequest request);
    }
}
