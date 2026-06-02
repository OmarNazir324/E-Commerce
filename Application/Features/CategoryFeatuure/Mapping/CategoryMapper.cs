using Application.Features.CategoryFeatuure.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeatuure.Mapping
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Category,CategoryDTO>().ForMember(b=> b.Main_Category_Name,f=> f.MapFrom(mapExpression=> mapExpression.ParentCategory.Name)).ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();

        }
    }
}
