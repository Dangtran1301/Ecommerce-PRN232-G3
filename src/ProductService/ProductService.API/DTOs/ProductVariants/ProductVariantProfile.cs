using AutoMapper;
using CatalogService.API.DTOs;
using CatalogService.Entities;

namespace CatalogService.API.Mappings
{
    public class ProductVariantProfile : Profile
    {
        public ProductVariantProfile()
        {
            CreateMap<ProductVariant, ProductVariantDto>();
            CreateMap<CreateProductVariantRequest, ProductVariant>();
            CreateMap<UpdateProductVariantRequest, ProductVariant>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcValue) => srcValue != null));
        }
    }
}
