using AutoMapper;
using OrderService.API.DTOs;
using OrderService.API.Models;

namespace OrderService.API.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<OrderItem, OrderItemDto>();

            CreateMap<CreateOrderRequest, Order>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Items.Sum(i => i.Price * i.Quantity)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => OrderStatus.Pending));

            CreateMap<CreateOrderItemRequest, OrderItem>();

            CreateMap<UpdateOrderRequest, Order>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcValue) => srcValue != null));

            CreateMap<UpdateOrderItemRequest, OrderItem>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcValue) => srcValue != null));
        }
    }
}