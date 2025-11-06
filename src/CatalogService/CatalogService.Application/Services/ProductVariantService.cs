using AutoMapper;
using CatalogService.API.DTOs;
using CatalogService.API.Errors;
using CatalogService.API.Repositories.Interfaces;
using CatalogService.API.Services.Interfaces;
using CatalogService.API.Specifications;
using CatalogService.Entities;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.API.Services
{
    public class ProductVariantService(
        IProductVariantRepository repository,
        ISpecificationRepository<ProductVariant> specificationRepository,
        IDynamicRepository<ProductVariant> dynamicRepository,
        IMapper mapper
    ) : IProductVariantService
    {
        public async Task<Result<ProductVariantDto>> GetByIdAsync(Guid id)
        {
            var variant = await repository.GetByIdAsync(id);
            return variant is null
                ? ProductVariantErrors.NotFound(id)
                : mapper.Map<ProductVariantDto>(variant);
        }

        public async Task<Result<IReadOnlyList<ProductVariantDto>>> GetAllAsync()
        {
            var list = await repository.GetAllAsync();
            return Result.Ok(mapper.Map<IReadOnlyList<ProductVariantDto>>(list));
        }

        public async Task<Result> CreateAsync(CreateProductVariantRequest request)
        {
            var exists = await repository.AnyAsync(v =>
                v.ProductId == request.ProductId &&
                v.VariantName == request.VariantName);

            if (exists)
                return ProductVariantErrors.NameTaken(request.VariantName);

            var entity = mapper.Map<ProductVariant>(request);
            await repository.AddAsync(entity);
            return true;
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateProductVariantRequest request)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null)
                return ProductVariantErrors.NotFound(id);

            mapper.Map(request, entity);
            await repository.Update(entity);
            return true;
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null)
                return ProductVariantErrors.NotFound(id);

            await repository.Remove(entity);
            return true;
        }

        public async Task<Result<IReadOnlyList<ProductVariantDto>>> FilterBySpecification(ProductVariantFilterDto filter)
        {
            var spec = new ProductVariantFilterSpecification(filter);
            var list = await specificationRepository.ListAsync(spec);
            return Result.Ok(mapper.Map<IReadOnlyList<ProductVariantDto>>(list));
        }

        public async Task<Result<PagedResult<ProductVariantDto>>> FilterByDynamic(DynamicQuery query)
        {
            var result = await dynamicRepository.GetPagedAsync(query);
            var dto = result.Map(mapper.Map<IReadOnlyList<ProductVariantDto>>(result.Items));
            return Result.Ok(dto);
        }

        public async Task<Result<PagedResult<ProductVariantDto>>> FilterPaged(PagedRequest request)
        {
            var result = await repository.GetPagedAsync(request);
            var dto = result.Map(mapper.Map<IReadOnlyList<ProductVariantDto>>(result.Items));
            return Result.Ok(dto);
        }
    }
}