using AutoMapper;
using CatalogService.Application.DTOs.ProductVariants;
using CatalogService.Application.Errors;
using CatalogService.Application.Services.Interfaces;
using CatalogService.Entities;
using CatalogService.Infrastructure.Repositories.Interfaces;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.Application.Services
{
    public class ProductVariantService(
        IProductVariantRepository productVariantRepository,
        ISpecificationRepository<ProductVariant> specificationRepository,
        IDynamicRepository<ProductVariant> dynamicRepository,
        IMapper mapper
    ) : IProductVariantService
    {
        public async Task<Result<ProductVariantDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var productVariant = await productVariantRepository.GetByIdAsync(id, cancellationToken);
            return productVariant is null
                ? ProductVariantErrors.NotFound(id)
                : mapper.Map<ProductVariantDto>(productVariant);
        }

        public async Task<Result<IReadOnlyList<ProductVariantDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var productVariants = await productVariantRepository.GetAllAsync(cancellationToken);
            return Result.Ok(mapper.Map<IReadOnlyList<ProductVariantDto>>(productVariants));
        }

        public async Task<Result<ProductVariantDto>> CreateAsync(CreateProductVariantRequest request, CancellationToken cancellationToken = default)
        {
            request.VariantName = request.VariantName.Trim();
            request.Sku = request.Sku?.Trim();
            request.ImageUrl = request.ImageUrl?.Trim();

            var productVariant = mapper.Map<ProductVariant>(request);
            await productVariantRepository.AddAsync(productVariant, cancellationToken);
            
            // Reload the created variant to return it
            var createdVariant = await productVariantRepository.GetByIdAsync(productVariant.Id, cancellationToken);
            if (createdVariant == null)
                return ProductVariantErrors.NotFound(productVariant.Id);
            
            return mapper.Map<ProductVariantDto>(createdVariant);
        }

        public async Task<Result<ProductVariantDto>> UpdateAsync(Guid id, UpdateProductVariantRequest request, CancellationToken cancellationToken = default)
        {
            var productVariant = await productVariantRepository.GetByIdAsync(id, cancellationToken);
            if (productVariant is null)
                return ProductVariantErrors.NotFound(id);

            request.VariantName = request.VariantName.Trim();
            request.Sku = request.Sku?.Trim();
            request.ImageUrl = request.ImageUrl?.Trim();

            mapper.Map(request, productVariant);
            await productVariantRepository.Update(productVariant, cancellationToken);
            
            // Reload the updated variant to return it
            var updatedVariant = await productVariantRepository.GetByIdAsync(id, cancellationToken);
            if (updatedVariant == null)
                return ProductVariantErrors.NotFound(id);
            
            return mapper.Map<ProductVariantDto>(updatedVariant);
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var productVariant = await productVariantRepository.GetByIdAsync(id, cancellationToken);
            if (productVariant is null)
                return ProductVariantErrors.NotFound(id);

            await productVariantRepository.Remove(productVariant, cancellationToken);
            return true;
        }

        public async Task<Result<IReadOnlyList<ProductVariantDto>>> FilterBySpecification(ProductVariantFilterDto filter, CancellationToken cancellationToken = default)
        {
            var spec = new ProductVariantFilterSpecification(filter);
            var productVariants = await specificationRepository.ListAsync(spec, cancellationToken);
            return Result.Ok(mapper.Map<IReadOnlyList<ProductVariantDto>>(productVariants));
        }

        public async Task<Result<PagedResult<ProductVariantDto>>> FilterPaged(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await productVariantRepository.GetPagedAsync(request, cancellationToken);
            var dto = result.Map(mapper.Map<IReadOnlyList<ProductVariantDto>>(result.Items));
            return Result.Ok(dto);
        }

        public IQueryable<ProductVariantDto> AsQueryable()
        {
            return productVariantRepository.GetQueryable().Select(v => new ProductVariantDto
            {
                Id = v.Id,
                ProductId = v.ProductId,
                VariantName = v.VariantName,
                Price = v.Price,
                Sku = v.Sku,
                ImageUrl = v.ImageUrl
            });
        }
    }
}