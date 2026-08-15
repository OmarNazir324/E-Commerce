using Application.Features.CategoryFeatuure.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.CategoryFeatuure.Mapping;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<Category, CreateCategoryDTO>().ReverseMap();
        CreateMap<Category, CategoryDTO>().ForMember(b => b.Main_Category_Name, f => f.MapFrom(mapExpression => mapExpression.ParentCategory.Name)).ReverseMap();
        CreateMap<Category, UpdateCategoryDTO>().ReverseMap();

    }
}
