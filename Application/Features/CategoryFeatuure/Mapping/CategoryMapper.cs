using Application.Features.CategoryFeatuure.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.CategoryFeatuure.Mapping;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<Category, CreateCategoryDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ForMember(b => b.Main_Category_Name, f => f.MapFrom(mapExpression => mapExpression.ParentCategory.Name)).ReverseMap();
        CreateMap<Category, UpdateCategoryDto>().ReverseMap();

    }
}
