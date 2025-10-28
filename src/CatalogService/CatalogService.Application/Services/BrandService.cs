using AutoMapper;
using CatalogService.Application.DTOs;
using CatalogService.Application.Errors;
using CatalogService.Application.Services.Interfaces;
using CatalogService.Infrastructure.Repositories.Interfaces;
using CatalogService.Domain.Entities;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using CatalogService.Application.DTOs.Brands;

namespace CatalogService.Application.Services
{
    public class BrandService(
        IBrandRepository repository,
        ISpecificationRepository<Brand> specificationRepository,
        IDynamicRepository<Brand> dynamicRepository,
        IMapper mapper
    ) : IBrandService
    {
        public async Task<Result<BrandDto>> GetByIdAsync(Guid id)
        {
            var brand = await repository.GetByIdAsync(id);
            return brand is null
                ? BrandErrors.NotFound(id)
                : mapper.Map<BrandDto>(brand);
        }

        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllAsync()
        {
            var brands = await repository.GetAllAsync();
            return Result.Ok(mapper.Map<IReadOnlyList<BrandDto>>(brands));
        }

        public async Task<Result> CreateAsync(CreateBrandRequest request)
        {
            request.BrandName = request.BrandName.Trim();
            request.BrandDescription = request.BrandDescription?.Trim();
            request.WebsiteUrl = request.WebsiteUrl?.Trim();
            if (await repository.AnyAsync(b => b.BrandName == request.BrandName))
                return BrandErrors.NameTaken(request.BrandName);

            var brand = mapper.Map<Brand>(request);
            await repository.AddAsync(brand);
            return true;
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateBrandRequest request)
        {
            var brand = await repository.GetByIdAsync(id);
            request.BrandName = request.BrandName.Trim();
            request.BrandDescription = request.BrandDescription?.Trim();
            request.WebsiteUrl = request.WebsiteUrl?.Trim();
            if (brand is null)
                return BrandErrors.NotFound(id);

            mapper.Map(request, brand);
            await repository.Update(brand);
            return true;
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var brand = await repository.GetByIdAsync(id);
            if (brand is null)
                return BrandErrors.NotFound(id);

            await repository.Remove(brand);
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
            var result = await repository.GetPagedAsync(request);
            var dto = result.Map(mapper.Map<IReadOnlyList<BrandDto>>(result.Items));
            return Result.Ok(dto);
        }
    }
}
