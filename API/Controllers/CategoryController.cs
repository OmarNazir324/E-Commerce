using Application.Exceptions;
using Application.Features.CategoryFeatuure.DTOs;
using Application.Features.CategoryFeatuure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)=>_categoryService = categoryService;
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAll();
            if (result == null) throw new NotFoundException("No Categories Was Retrived");
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result= await _categoryService.GetByid(id);
            if (result is null) return NotFound();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDTO createCategoryDTO)
        {
            await _categoryService.Create(createCategoryDTO);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryDTO updateCategoryDTO)
        {
            await _categoryService.Update(updateCategoryDTO);
            return Ok();
        }
        [HttpDelete("{id}")]
        public  async Task<IActionResult> Delete(int id)
        {
            await _categoryService.Delete(id);
            return Ok();
        }
    }
}
