using Application.CrudServiceGeneric;
using Application.Features.CategoryFeatuure.DTOs;
using Domain.Entities;
namespace Application.Features.CategoryFeatuure.Interfaces;

public interface ICategoryService:ImainServiceCRUD<CreateCategoryDTO,UpdateCategoryDTO,Category>
{
    Task<IEnumerable<CategoryDTO>> GetAll();
    Task<CategoryDTO> GetByid(int id);
}
