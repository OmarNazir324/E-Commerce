using Application.CrudServiceGeneric;
using Application.Features.CategoryFeatuure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeatuure.Interfaces
{
    public interface ICategoryService:ImainServiceCRUD<CreateCategoryDTO,UpdateCategoryDTO>
    {
        Task<IEnumerable<CategoryDTO>> GetAll();
        Task<CategoryDTO> GetByid(int id);
    }
}
