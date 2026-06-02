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
            .ReverseMap();
        CreateMap<Order, CreateOrderDTO>().ReverseMap();
        CreateMap<Order, UpdateOrderDTO>().ReverseMap();
    }
}
