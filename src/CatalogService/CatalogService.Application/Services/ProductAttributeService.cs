using AutoMapper;
using CatalogService.API.DTOs;
using CatalogService.API.Errors;
using CatalogService.API.Repositories.Interfaces;
using CatalogService.API.Services.Interfaces;
using CatalogService.Entities;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.API.Services
{
    public class ProductAttributeService(
        IProductAttributeRepository repository,
        ISpecificationRepository<ProductAttribute> specificationRepository,
        IDynamicRepository<ProductAttribute> dynamicRepository,
        IMapper mapper
    ) : IProductAttributeService
    {
        public async Task<Result<ProductAttributeDto>> GetByIdAsync(Guid id)
        {
            var attr = await repository.GetByIdAsync(id);
            return attr is null
                ? ProductAttributeErrors.NotFound(id)
                : mapper.Map<ProductAttributeDto>(attr);
        }

        public async Task<Result<IReadOnlyList<ProductAttributeDto>>> GetAllAsync()
        {
            var list = await repository.GetAllAsync();
            return Result.Ok(mapper.Map<IReadOnlyList<ProductAttributeDto>>(list));
        }

        public async Task<Result> CreateAsync(CreateProductAttributeRequest request)
        {
            var exists = await repository.AnyAsync(a =>
                a.ProductId == request.ProductId &&
                a.AttributeName == request.AttributeName);

            if (exists)
                return ProductAttributeErrors.NameTaken(request.AttributeName);

            var entity = mapper.Map<ProductAttribute>(request);
            await repository.AddAsync(entity);
            return true;
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateProductAttributeRequest request)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null)
                return ProductAttributeErrors.NotFound(id);

            mapper.Map(request, entity);
            await repository.Update(entity);
            return true;
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null)
                return ProductAttributeErrors.NotFound(id);

            await repository.Remove(entity);
            return true;
        }

        //public async Task<Result<IReadOnlyList<ProductAttributeDto>>> FilterBySpecification(ProductAttributeFilterDto filter)
        //{
        //    var spec = new ProductAttributeFilterSpecification(filter);
        //    var list = await specificationRepository.ListAsync(spec);
        //    return Result.Success(mapper.Map<IReadOnlyList<ProductAttributeDto>>(list));
        //}

        public async Task<Result<PagedResult<ProductAttributeDto>>> FilterByDynamic(DynamicQuery query)
        {
            var result = await dynamicRepository.GetPagedAsync(query);
            var dto = result.Map(mapper.Map<IReadOnlyList<ProductAttributeDto>>(result.Items));
            return Result.Ok(dto);
        }

        public async Task<Result<PagedResult<ProductAttributeDto>>> FilterPaged(PagedRequest request)
        {
            var result = await repository.GetPagedAsync(request);
            var dto = result.Map(mapper.Map<IReadOnlyList<ProductAttributeDto>>(result.Items));
            return Result.Ok(dto);
        }
    }
}