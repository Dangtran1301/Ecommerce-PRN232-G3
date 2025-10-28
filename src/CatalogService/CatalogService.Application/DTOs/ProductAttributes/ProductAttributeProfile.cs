using AutoMapper;
using CatalogService.API.DTOs;
using CatalogService.Entities;

namespace CatalogService.API.Mappings
{
    public class ProductAttributeProfile : Profile
    {
        public ProductAttributeProfile()
        {
            CreateMap<ProductAttribute, ProductAttributeDto>();
            CreateMap<CreateProductAttributeRequest, ProductAttribute>();
            CreateMap<UpdateProductAttributeRequest, ProductAttribute>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcValue) => srcValue != null));
        }
    }
}
 