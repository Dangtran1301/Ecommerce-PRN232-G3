using CatalogService.Application.DTOs.ProductAttributes;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.Application.Services.Interfaces;

public interface IProductAttributeService
{
    Task<Result<ProductAttributeDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<ProductAttributeDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result> CreateAsync(CreateProductAttributeRequest request, CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(Guid id, UpdateProductAttributeRequest request, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<ProductAttributeDto>>> FilterBySpecification(ProductAttributeFilterDto filter, CancellationToken cancellationToken = default);

    Task<Result<PagedResult<ProductAttributeDto>>> FilterPaged(PagedRequest request, CancellationToken cancellationToken = default);

    IQueryable<ProductAttributeDto> AsQueryable();
}