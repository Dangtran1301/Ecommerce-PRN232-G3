using AutoMapper;
using CatalogService.Domain.Entities;

namespace CatalogService.Application.DTOs
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcValue) => srcValue != null));
        }
    }
}
