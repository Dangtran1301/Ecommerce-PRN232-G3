using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using CatalogService.API.DTOs;

namespace CatalogService.API.Services.Interfaces
{
    public interface IProductVariantService
    {
        Task<Result<ProductVariantDto>> GetByIdAsync(Guid id);
        Task<Result<IReadOnlyList<ProductVariantDto>>> GetAllAsync();
        Task<Result> CreateAsync(CreateProductVariantRequest request);
        Task<Result> UpdateAsync(Guid id, UpdateProductVariantRequest request);
        Task<Result> DeleteAsync(Guid id);
        Task<Result<IReadOnlyList<ProductVariantDto>>> FilterBySpecification(ProductVariantFilterDto filter);
        Task<Result<PagedResult<ProductVariantDto>>> FilterByDynamic(DynamicQuery query);
        Task<Result<PagedResult<ProductVariantDto>>> FilterPaged(PagedRequest request);
    }
}
