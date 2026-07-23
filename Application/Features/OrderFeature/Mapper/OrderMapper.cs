using Application.Features.OrderFeature.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.OrderFeature.Mapper;

public class OrderMapper:Profile
{
    public OrderMapper()
    {
        CreateMap<Order, OrderDTO>()
            .ForMember(f => f.Customer_Name, m => m.MapFrom(b => b.Customer.Name))
            .ForMember(f=> f.Order_ItemsDTOs,m=> m.MapFrom(o=> o.Order_Items))
            .ReverseMap();
        CreateMap<Order, CreateOrderDTO>().ForMember(f => f.CreateOrder_Items, m => m.MapFrom(o => o.Order_Items)).ReverseMap();
        CreateMap<Order, UpdateOrderDTO>().ForMember(f => f.UpdateOrder_Items, m => m.MapFrom(o => o.Order_Items)).ReverseMap();
    }
}
