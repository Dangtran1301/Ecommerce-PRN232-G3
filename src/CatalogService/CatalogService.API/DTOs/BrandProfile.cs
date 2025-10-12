using AutoMapper;
using CatalogService.API.Entities;

namespace CatalogService.API.DTOs
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, BrandDto>();
            CreateMap<CreateBrandRequest, Brand>();
            CreateMap<UpdateBrandRequest, Brand>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcValue) => srcValue != null));
        }
    }
}
