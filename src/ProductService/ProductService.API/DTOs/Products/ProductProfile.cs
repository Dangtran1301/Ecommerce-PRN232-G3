using AutoMapper;
using CatalogService.API.DTOs;
using CatalogService.Entities;

namespace CatalogService.API.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.Stock.Quantity));

            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcValue) => srcValue != null));
        }
    }
}
