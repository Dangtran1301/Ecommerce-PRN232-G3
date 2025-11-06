using AutoMapper;
using CatalogService.Application.DTOs.Products;
using CatalogService.Application.Errors;
using CatalogService.Application.Services.Interfaces;
using CatalogService.Domain.Entities;
using CatalogService.Entities;
using CatalogService.Infrastructure.Repositories.Interfaces;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.Application.Services
{
    public class ProductAppService(
        IProductRepository productRepository,
        ISpecificationRepository<Product> specificationRepository,
        IDynamicRepository<Product> dynamicRepository,
        IMapper mapper
    ) : IProductService
    {
        public async Task<Result<ProductDto>> GetByIdAsync(Guid id)
        {
            var product = await productRepository.GetByIdAsync(id);
            return product is null
                ? ProductErrors.NotFound(id)
                : mapper.Map<ProductDto>(product);
        }

        public async Task<Result<IReadOnlyList<ProductDto>>> GetAllAsync()
        {
            var products = await productRepository.GetAllAsync();
            return Result.Ok(mapper.Map<IReadOnlyList<ProductDto>>(products));
        }

        public async Task<Result> CreateAsync(CreateProductRequest request)
        {
            request.ProductName = request.ProductName.Trim();
            if (await productRepository.AnyAsync(p => p.ProductName == request.ProductName))
                return ProductErrors.NameTaken(request.ProductName);

            var product = mapper.Map<Product>(request);
            await productRepository.AddAsync(product);
            return true;
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateProductRequest request)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
                return ProductErrors.NotFound(id);

            request.ProductName = request.ProductName.Trim();
            mapper.Map(request, product);
            await productRepository.Update(product);
            return true;
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
                return ProductErrors.NotFound(id);

            await productRepository.Remove(product);
            return true;
        }

        public async Task<Result<IReadOnlyList<ProductDto>>> FilterBySpecification(ProductFilterDto filter)
        {
            var spec = new ProductFilterSpecification(filter);
            var products = await specificationRepository.ListAsync(spec);
            return Result.Ok(mapper.Map<IReadOnlyList<ProductDto>>(products));
        }

        public async Task<Result<PagedResult<ProductDto>>> FilterPaged(PagedRequest request)
        {
            var result = await productRepository.GetPagedAsync(request);
            var dto = result.Map(mapper.Map<IReadOnlyList<ProductDto>>(result.Items));
            return Result.Ok(dto);
        }

        public IQueryable<ProductDto> AsQueryable()
        {
            return productRepository.GetQueryable().Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                Sku = p.Sku,
                ImageUrl = p.ImageUrl,
                Specifications = p.Specifications,
                BrandId = p.BrandId,
                BrandName = p.Brand.BrandName,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.CategoryName
            });
        }
    }
}
