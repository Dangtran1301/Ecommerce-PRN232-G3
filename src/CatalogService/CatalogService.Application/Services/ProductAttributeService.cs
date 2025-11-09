using AutoMapper;
using CatalogService.Application.DTOs.ProductAttributes;
using CatalogService.Application.Errors;
using CatalogService.Application.Services.Interfaces;
using CatalogService.Entities;
using CatalogService.Infrastructure.Repositories.Interfaces;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.Application.Services;

public class ProductAttributeService(
    IProductAttributeRepository productAttributeRepository,
    ISpecificationRepository<ProductAttribute> specificationRepository,
    IDynamicRepository<ProductAttribute> dynamicRepository,
    IMapper mapper
) : IProductAttributeService
{
    public async Task<Result<ProductAttributeDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var productAttribute = await productAttributeRepository.GetByIdAsync(id, cancellationToken);
        return productAttribute is null
            ? ProductAttributeErrors.NotFound(id)
            : mapper.Map<ProductAttributeDto>(productAttribute);
    }

    public async Task<Result<IReadOnlyList<ProductAttributeDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var productAttributes = await productAttributeRepository.GetAllAsync(cancellationToken);
        return Result.Ok(mapper.Map<IReadOnlyList<ProductAttributeDto>>(productAttributes));
    }

    public async Task<Result<ProductAttributeDto>> CreateAsync(CreateProductAttributeRequest request, CancellationToken cancellationToken = default)
    {
        request.AttributeName = request.AttributeName.Trim();
        request.AttributeValue = request.AttributeValue.Trim();

        if (await productAttributeRepository.AnyAsync(a =>
            a.ProductId == request.ProductId &&
            a.AttributeName == request.AttributeName,
            cancellationToken))
            return ProductAttributeErrors.NameTaken(request.AttributeName);

        var productAttribute = mapper.Map<ProductAttribute>(request);
        await productAttributeRepository.AddAsync(productAttribute, cancellationToken);
        
        // Reload the created attribute to return it
        var createdAttribute = await productAttributeRepository.GetByIdAsync(productAttribute.Id, cancellationToken);
        if (createdAttribute == null)
            return ProductAttributeErrors.NotFound(productAttribute.Id);
        
        return mapper.Map<ProductAttributeDto>(createdAttribute);
    }

    public async Task<Result<ProductAttributeDto>> UpdateAsync(Guid id, UpdateProductAttributeRequest request, CancellationToken cancellationToken = default)
    {
        var productAttribute = await productAttributeRepository.GetByIdAsync(id, cancellationToken);
        if (productAttribute is null)
            return ProductAttributeErrors.NotFound(id);

        request.AttributeName = request.AttributeName.Trim();
        request.AttributeValue = request.AttributeValue.Trim();

        mapper.Map(request, productAttribute);
        await productAttributeRepository.Update(productAttribute, cancellationToken);
        
        // Reload the updated attribute to return it
        var updatedAttribute = await productAttributeRepository.GetByIdAsync(id, cancellationToken);
        if (updatedAttribute == null)
            return ProductAttributeErrors.NotFound(id);
        
        return mapper.Map<ProductAttributeDto>(updatedAttribute);
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var productAttribute = await productAttributeRepository.GetByIdAsync(id, cancellationToken);
        if (productAttribute is null)
            return ProductAttributeErrors.NotFound(id);

        await productAttributeRepository.Remove(productAttribute, cancellationToken);
        return true;
    }

    public async Task<Result<IReadOnlyList<ProductAttributeDto>>> FilterBySpecification(ProductAttributeFilterDto filter, CancellationToken cancellationToken = default)
    {
        var spec = new ProductAttributeFilterSpecification(filter);
        var productAttributes = await specificationRepository.ListAsync(spec, cancellationToken);
        return Result.Ok(mapper.Map<IReadOnlyList<ProductAttributeDto>>(productAttributes));
    }

    public async Task<Result<PagedResult<ProductAttributeDto>>> FilterPaged(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await productAttributeRepository.GetPagedAsync(request, cancellationToken);
        var dto = result.Map(mapper.Map<IReadOnlyList<ProductAttributeDto>>(result.Items));
        return Result.Ok(dto);
    }

    public IQueryable<ProductAttributeDto> AsQueryable()
    {
        return productAttributeRepository.GetQueryable().Select(a => new ProductAttributeDto
        {
            Id = a.Id,
            ProductId = a.ProductId,
            AttributeName = a.AttributeName,
            AttributeValue = a.AttributeValue
        });
    }
}