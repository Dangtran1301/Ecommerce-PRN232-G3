using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using CatalogService.Application.DTOs;

namespace CatalogService.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<CategoryDto>> GetByIdAsync(Guid id);

        Task<Result<IReadOnlyList<CategoryDto>>> GetAllAsync();

        Task<Result> CreateAsync(CreateCategoryRequest request);

        Task<Result> UpdateAsync(Guid id, UpdateCategoryRequest request);

        Task<Result> DeleteAsync(Guid id);

        Task<Result<IReadOnlyList<CategoryDto>>> FilterBySpecification(CategoryFilterDto filter);

        Task<Result<PagedResult<CategoryDto>>> FilterByDynamic(DynamicQuery query);

        Task<Result<PagedResult<CategoryDto>>> FilterPaged(PagedRequest request);
    }
}
