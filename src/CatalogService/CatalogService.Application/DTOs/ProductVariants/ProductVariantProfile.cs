using AutoMapper;
using CatalogService.Entities;

namespace CatalogService.Application.DTOs.ProductVariants
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
