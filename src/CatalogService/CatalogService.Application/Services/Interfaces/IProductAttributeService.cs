using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.API.Services.Interfaces
{
    public interface IProductAttributeService
    {
        Task<Result<ProductAttributeDto>> GetByIdAsync(Guid id);
        Task<Result<IReadOnlyList<ProductAttributeDto>>> GetAllAsync();
        Task<Result> CreateAsync(CreateProductAttributeRequest request);
        Task<Result> UpdateAsync(Guid id, UpdateProductAttributeRequest request);
        Task<Result> DeleteAsync(Guid id);
        //Task<Result<IReadOnlyList<ProductAttributeDto>>> FilterBySpecification(ProductAttributeFilterDto filter);
        Task<Result<PagedResult<ProductAttributeDto>>> FilterByDynamic(DynamicQuery query);
        Task<Result<PagedResult<ProductAttributeDto>>> FilterPaged(PagedRequest request);
    }
}
