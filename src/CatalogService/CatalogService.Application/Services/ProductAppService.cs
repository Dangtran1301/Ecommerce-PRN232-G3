using AutoMapper;
using CatalogService.API.DTOs;
using CatalogService.API.Errors;
using CatalogService.API.Repositories.Interfaces;
using CatalogService.API.Services.Interfaces;
using CatalogService.API.Specifications;
using CatalogService.Entities;
using CatalogService.Infrastructure.Repositories.Interfaces;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.API.Services
{
    public class ProductAppService(ICategoryRepository categoryRepository,
        IProductRepository repository,
        ISpecificationRepository<Product> specificationRepository,
        IDynamicRepository<Product> dynamicRepository,
        IMapper mapper
    ) : IProductService
    {
        public async Task<Result<ProductDto>> GetByIdAsync(Guid id)
        {
            var product = await repository.GetByIdWithRelationsAsync(id);
            return product is null
                ? ProductErrors.NotFound(id)
                : mapper.Map<ProductDto>(product);
        }

        public async Task<Result<IReadOnlyList<ProductDto>>> GetAllAsync()
        {
            var products = await repository.GetAllWithRelationsAsync();
            return Result.Ok(mapper.Map<IReadOnlyList<ProductDto>>(products));
        }

        //public async Task<Result> CreateAsync(CreateProductRequest request)
        //{
        //    if (await repository.AnyAsync(p => p.ProductName == request.ProductName))
        //        return ProductErrors.NameTaken(request.ProductName);

        //    var entity = mapper.Map<Product>(request);
        //    await repository.AddAsync(entity);
        //    return true;
        //}
        public async Task<Result> CreateAsync(CreateProductRequest request)
        {
            if (await repository.AnyAsync(p => p.ProductName == request.ProductName))
                return ProductErrors.NameTaken(request.ProductName);

            if (request.CategoryId == null)
                return ProductErrors.MissingRelations("category");

            if (!await categoryRepository.AnyAsync(c => c.Id == request.CategoryId.Value))
                return ProductErrors.CategoryNotFound(request.CategoryId.ToString()!);

            var entity = mapper.Map<Product>(request);
            await repository.AddAsync(entity);
            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateProductRequest request)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null)
                return ProductErrors.NotFound(id);

            mapper.Map(request, entity);
            await repository.Update(entity);
            return true;
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null)
                return ProductErrors.NotFound(id);

            await repository.Remove(entity);
            return true;
        }

        public async Task<Result<IReadOnlyList<ProductDto>>> FilterBySpecification(ProductFilterDto filter)
        {
            var spec = new ProductFilterSpecification(filter);
            var list = await specificationRepository.ListAsync(spec);
            return Result.Ok(mapper.Map<IReadOnlyList<ProductDto>>(list));
        }

        public async Task<Result<PagedResult<ProductDto>>> FilterByDynamic(DynamicQuery query)
        {
            var result = await dynamicRepository.GetPagedAsync(query);
            var dto = result.Map(mapper.Map<IReadOnlyList<ProductDto>>(result.Items));
            return Result.Ok(dto);
        }

        public async Task<Result<PagedResult<ProductDto>>> FilterPaged(PagedRequest request)
        {
            var result = await repository.GetPagedAsync(request);
            var dto = result.Map(mapper.Map<IReadOnlyList<ProductDto>>(result.Items));
            return Result.Ok(dto);
        }
    }
}