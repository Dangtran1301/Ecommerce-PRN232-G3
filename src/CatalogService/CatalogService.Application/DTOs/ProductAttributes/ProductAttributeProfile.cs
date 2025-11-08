using AutoMapper;
using CatalogService.Entities;

namespace CatalogService.Application.DTOs.ProductAttributes;

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