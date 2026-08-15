using Application.CrudServiceGeneric;
using Application.Features.CategoryFeatuure.DTOs;
using Domain.Entities;
namespace Application.Features.CategoryFeatuure.Interfaces;

public interface ICategoryService:ImainServiceCRUD<CreateCategoryDto, UpdateCategoryDto, Category>
{
    Task<IEnumerable<CategoryDto>> GetAll();
    Task<CategoryDto> GetByid(int id);
}
