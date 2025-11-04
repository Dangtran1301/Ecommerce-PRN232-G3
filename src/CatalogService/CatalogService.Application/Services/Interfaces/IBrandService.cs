using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using CatalogService.Application.DTOs.Brands;

namespace CatalogService.Application.Services.Interfaces
{
    public interface IBrandService
    {
        Task<Result<BrandDto>> GetByIdAsync(Guid id);

        Task<Result<IReadOnlyList<BrandDto>>> GetAllAsync();

        Task<Result> CreateAsync(CreateBrandRequest request);

        Task<Result> UpdateAsync(Guid id, UpdateBrandRequest request);

        Task<Result> DeleteAsync(Guid id);
        IQueryable<BrandDto> AsQueryable();
    }
}
