using AutoMapper;
using CatalogService.Domain.Entities;
using CatalogService.Entities;

namespace CatalogService.Application.DTOs.Stocks
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
