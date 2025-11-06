using AutoMapper;
using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.Errors;
using CatalogService.Application.Services.Interfaces;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Repositories.Interfaces;
using SharedKernel.Application.Common;
using SharedKernel.Domain.Common.Results;
using SharedKernel.Infrastructure.UnitOfWorks.Interfaces;

namespace CatalogService.Application.Services
{
    public class CategoryService(
    ICategoryRepository categoryRepository,
    IProductRepository productRepository,
    ISpecificationRepository<Category> specificationRepository,
    IDynamicRepository<Category> dynamicRepository,
    IMapper mapper
) : ICategoryService
    {
        public async Task<Result<CategoryDto>> GetByIdAsync(Guid id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            return category is null
                ? CategoryErrors.NotFound(id)
                : mapper.Map<CategoryDto>(category);
        }

        public async Task<Result<IReadOnlyList<CategoryDto>>> GetAllAsync()
        {
            var categories = await categoryRepository.GetAllAsync();
            return Result.Ok(mapper.Map<IReadOnlyList<CategoryDto>>(categories));
        }

        public async Task<Result> CreateAsync(CreateCategoryRequest request)
        {
            request.CategoryName = request.CategoryName.Trim();
            request.CategoryDescription = request.CategoryDescription.Trim();
            if (await categoryRepository.AnyAsync(c => c.CategoryName == request.CategoryName))
                return CategoryErrors.NameTaken(request.CategoryName);

            var category = mapper.Map<Category>(request);
            await categoryRepository.AddAsync(category);
            return true;
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateCategoryRequest request)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            request.CategoryName = request.CategoryName.Trim();
            request.CategoryDescription = request.CategoryDescription.Trim();
            if (category is null)
                return CategoryErrors.NotFound(id);

            mapper.Map(request, category);
            await categoryRepository.Update(category);
            return true;
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category is null)
                return CategoryErrors.NotFound(id);
            bool hasProducts = await productRepository.AnyAsync(p => p.CategoryId == id);
            if (hasProducts)
                return CategoryErrors.HasProducts(id);
            await categoryRepository.Remove(category);
            return true;
        }

        public async Task<Result<IReadOnlyList<CategoryDto>>> FilterBySpecification(CategoryFilterDto filter)
        {
            var spec = new CategoryFilterSpecification(filter);
            var categories = await specificationRepository.ListAsync(spec);
            return Result.Ok(mapper.Map<IReadOnlyList<CategoryDto>>(categories));
        }

        public async Task<Result<PagedResult<CategoryDto>>> FilterPaged(PagedRequest request)
        {
            var result = await categoryRepository.GetPagedAsync(request);
            var dto = result.Map(mapper.Map<IReadOnlyList<CategoryDto>>(result.Items));
            return Result.Ok(dto);
        }

        public IQueryable<CategoryDto> AsQueryable()
        {
            return categoryRepository.GetQueryable().Select(c => new CategoryDto
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                CategoryDescription = c.CategoryDescription
            });
        }
    }
}