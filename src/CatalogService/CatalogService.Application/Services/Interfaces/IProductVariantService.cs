using CatalogService.Application.DTOs.ProductVariants;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.Application.Services.Interfaces
{
    public interface IProductVariantService
    {
        Task<Result<ProductVariantDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<ProductVariantDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result> CreateAsync(CreateProductVariantRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(Guid id, UpdateProductVariantRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<ProductVariantDto>>> FilterBySpecification(ProductVariantFilterDto filter, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<ProductVariantDto>>> FilterPaged(PagedRequest request, CancellationToken cancellationToken = default);
        IQueryable<ProductVariantDto> AsQueryable();
    }
}
