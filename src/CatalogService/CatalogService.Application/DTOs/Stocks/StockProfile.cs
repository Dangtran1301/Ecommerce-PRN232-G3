using AutoMapper;
using CatalogService.API.DTOs;
using CatalogService.Entities;

namespace CatalogService.API.Mappings
{
    public class StockProfile : Profile
    {
        public StockProfile()
        {
            CreateMap<Stock, StockDto>();
            CreateMap<CreateStockRequest, Stock>();
            CreateMap<UpdateStockRequest, Stock>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcValue) => srcValue != null));
        }
    }
}