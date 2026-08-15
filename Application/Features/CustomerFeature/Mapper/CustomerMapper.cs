using Application.Features.CustomerFeature.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.CustomerFeature.Mapper;

public class CustomerMapper:Profile
{
    public CustomerMapper()
    {
        CreateMap<Customer, CreateCustomerDto>().ReverseMap();
        CreateMap<Customer, CustomerDto>()
            .ForMember(x=> x.OrdersCount , m=> m.MapFrom(f=> f.Orders.Count()))
            .ReverseMap();
        CreateMap<Customer,UpdateCustomerDto>().ReverseMap();
    }
}
