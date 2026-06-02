using Application.Features.CustomerFeature.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.CustomerFeature.Mapper
{
    public class CustomerMapper:Profile
    {
        public CustomerMapper()
        {
            CreateMap<Customer, CreateCustomerDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Customer,UpdateCustomerDTO>().ReverseMap();
        }
    }
}
