using AutoMapper;
using CatalogService.API.Repositories.Interfaces;
using CatalogService.Application.DTOs.Brands;
using CatalogService.Application.Errors;
using CatalogService.Application.Services.Interfaces;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Repositories.Interfaces;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.Application.Services
{
    public class BrandService(
        IBrandRepository brandRepository,
        IProductRepository productRepository,
        ISpecificationRepository<Brand> specificationRepository,
        IDynamicRepository<Brand> dynamicRepository,
        IMapper mapper
    ) : IBrandService
    {
        public async Task<Result<BrandDto>> GetByIdAsync(Guid id)
        {
            var brand = await brandRepository.GetByIdAsync(id);
            return brand is null
                ? BrandErrors.NotFound(id)
                : mapper.Map<BrandDto>(brand);
        }

        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllAsync()
        {
            var brands = await brandRepository.GetAllAsync();
            return Result.Ok(mapper.Map<IReadOnlyList<BrandDto>>(brands));
        }

        public async Task<Result> CreateAsync(CreateBrandRequest request)
        {
            request.BrandName = request.BrandName.Trim();
            request.BrandDescription = request.BrandDescription?.Trim();
            request.WebsiteUrl = request.WebsiteUrl?.Trim();
            if (await brandRepository.AnyAsync(b => b.BrandName == request.BrandName))
                return BrandErrors.NameTaken(request.BrandName);

            var brand = mapper.Map<Brand>(request);
            await brandRepository.AddAsync(brand);
            return true;
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateBrandRequest request)
        {
            var brand = await brandRepository.GetByIdAsync(id);
            request.BrandName = request.BrandName.Trim();
            request.BrandDescription = request.BrandDescription?.Trim();
            request.WebsiteUrl = request.WebsiteUrl?.Trim();
            if (brand is null)
                return BrandErrors.NotFound(id);

            mapper.Map(request, brand);
            await brandRepository.Update(brand);
            return true;
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var brand = await brandRepository.GetByIdAsync(id);
            if (brand is null)
                return BrandErrors.NotFound(id);
            bool hasProducts = await productRepository.AnyAsync(p => p.BrandId == id);
            if (hasProducts)
                return BrandErrors.HasProducts(id);
            await brandRepository.Remove(brand);
            return true;
        }

        public async Task<Result<IReadOnlyList<BrandDto>>> FilterBySpecification(BrandFilterDto filter)
        {
            var spec = new BrandFilterSpecification(filter);
            var brands = await specificationRepository.ListAsync(spec);
            return Result.Ok(mapper.Map<IReadOnlyList<BrandDto>>(brands));
        }

        public async Task<Result<PagedResult<BrandDto>>> FilterPaged(PagedRequest request)
        {
            var result = await brandRepository.GetPagedAsync(request);
            var dto = result.Map(mapper.Map<IReadOnlyList<BrandDto>>(result.Items));
            return Result.Ok(dto);
        }

        public IQueryable<BrandDto> AsQueryable()
        {
            return brandRepository.GetQueryable().Select(b => new BrandDto
            {
                Id = b.Id,
                BrandName = b.BrandName,
                BrandDescription = b.BrandDescription,
                WebsiteUrl = b.WebsiteUrl,
            });
        }
    }
}