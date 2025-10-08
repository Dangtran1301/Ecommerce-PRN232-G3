using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using CatalogService.API.DTOs;

namespace CatalogService.API.Services.Interfaces
{
    public interface IBrandService
    {
        Task<Result<BrandDto>> GetByIdAsync(Guid id);

        Task<Result<IReadOnlyList<BrandDto>>> GetAllAsync();

        Task<Result> CreateAsync(CreateBrandRequest request);

        Task<Result> UpdateAsync(Guid id, UpdateBrandRequest request);

        Task<Result> DeleteAsync(Guid id);

        Task<Result<IReadOnlyList<BrandDto>>> FilterBySpecification(BrandFilterDto filter);

        Task<Result<PagedResult<BrandDto>>> FilterByDynamic(DynamicQuery query);

        Task<Result<PagedResult<BrandDto>>> FilterPaged(PagedRequest request);
    }
}
