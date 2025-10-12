using AutoMapper;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Repositories.Interfaces;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;
using CatalogService.Application.Services.Interfaces;
using CatalogService.Application.Errors;
using CatalogService.Application.DTOs;

namespace CatalogService.Application.Services
{
    public class CategoryService(
        ICategoryRepository repository,
        ISpecificationRepository<Category> specificationRepository,
        IDynamicRepository<Category> dynamicRepository,
        IMapper mapper
    ) : ICategoryService
    {
        public async Task<Result<CategoryDto>> GetByIdAsync(Guid id)
        {
            var category = await repository.GetByIdAsync(id);
            return category is null
                ? CategoryErrors.NotFound(id)
                : mapper.Map<CategoryDto>(category);
        }

        public async Task<Result<IReadOnlyList<CategoryDto>>> GetAllAsync()
        {
            var categories = await repository.GetAllAsync();
            return Result.Success(mapper.Map<IReadOnlyList<CategoryDto>>(categories));
        }

        public async Task<Result> CreateAsync(CreateCategoryRequest request)
        {
            if (await repository.AnyAsync(c => c.CategoryName == request.CategoryName))
                return CategoryErrors.NameTaken(request.CategoryName);

            var category = mapper.Map<Category>(request);
            await repository.AddAsync(category);
            return true;
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateCategoryRequest request)
        {
            var category = await repository.GetByIdAsync(id);
            if (category is null)
                return CategoryErrors.NotFound(id);

            mapper.Map(request, category);
            await repository.Update(category);
            return true;
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var category = await repository.GetByIdAsync(id);
            if (category is null)
                return CategoryErrors.NotFound(id);

            await repository.Remove(category);
            return true;
        }

        public async Task<Result<IReadOnlyList<CategoryDto>>> FilterBySpecification(CategoryFilterDto filter)
        {
            var spec = new CategoryFilterSpecification(filter);
            var categories = await specificationRepository.ListAsync(spec);
            return Result.Success(mapper.Map<IReadOnlyList<CategoryDto>>(categories));
        }

        public async Task<Result<PagedResult<CategoryDto>>> FilterByDynamic(DynamicQuery query)
        {
            var result = await dynamicRepository.GetPagedAsync(query);
            var dto = result.Map(mapper.Map<IReadOnlyList<CategoryDto>>(result.Items));
            return Result.Success(dto);
        }

        public async Task<Result<PagedResult<CategoryDto>>> FilterPaged(PagedRequest request)
        {
            var result = await repository.GetPagedAsync(request);
            var dto = result.Map(mapper.Map<IReadOnlyList<CategoryDto>>(result.Items));
            return Result.Success(dto);
        }
    }
}
