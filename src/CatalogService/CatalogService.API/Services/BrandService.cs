using AutoMapper;
using CatalogService.API.DTOs;
using CatalogService.API.Entities;
using CatalogService.API.Errors;
using CatalogService.API.Repositories.Interfaces;
using CatalogService.API.Services.Interfaces;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.API.Services
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
            return Result.Success(mapper.Map<IReadOnlyList<BrandDto>>(brands));
        }

        public async Task<Result> CreateAsync(CreateBrandRequest request)
        {
            if (await repository.AnyAsync(b => b.BrandName == request.BrandName))
                return BrandErrors.NameTaken(request.BrandName);

            var brand = mapper.Map<Brand>(request);
            await repository.AddAsync(brand);
            return true;
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateBrandRequest request)
        {
            var brand = await repository.GetByIdAsync(id);
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
            return Result.Success(mapper.Map<IReadOnlyList<BrandDto>>(brands));
        }

        public async Task<Result<PagedResult<BrandDto>>> FilterByDynamic(DynamicQuery query)
        {
            var result = await dynamicRepository.GetPagedAsync(query);
            var dto = result.Map(mapper.Map<IReadOnlyList<BrandDto>>(result.Items));
            return Result.Success(dto);
        }

        public async Task<Result<PagedResult<BrandDto>>> FilterPaged(PagedRequest request)
        {
            var result = await repository.GetPagedAsync(request);
            var dto = result.Map(mapper.Map<IReadOnlyList<BrandDto>>(result.Items));
            return Result.Success(dto);
        }
    }
}
