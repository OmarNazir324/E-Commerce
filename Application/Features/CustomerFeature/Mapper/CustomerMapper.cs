using Application.Features.CustomerFeature.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.CustomerFeature.Mapper;

public class CustomerMapper:Profile
{
    public CustomerMapper()
    {
        CreateMap<Customer, CreateCustomerDTO>().ReverseMap();
        CreateMap<Customer, CustomerDTO>()
            .ForMember(x=> x.OrdersCount , m=> m.MapFrom(f=> f.Orders.Count()))
            .ReverseMap();
        CreateMap<Customer,UpdateCustomerDTO>().ReverseMap();
    }
}
