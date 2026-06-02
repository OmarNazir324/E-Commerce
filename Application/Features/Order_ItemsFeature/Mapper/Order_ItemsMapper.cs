using Application.Features.Order_ItemsFeature.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Order_ItemsFeature.Mapper
{
    public class Order_ItemsMapper : Profile
    {
        public Order_ItemsMapper()
        {
            CreateMap<Order_items, Order_itemsDTO>()
                .ForMember(f => f.Product_Name, m => m.MapFrom(p => p.Name)).ReverseMap();
            CreateMap<Order_items, CreateOrder_itemsDTO>().ReverseMap();
            CreateMap<Order_items, UpdateOrder_ItemsDTO>().ReverseMap();
        }
    }
}
