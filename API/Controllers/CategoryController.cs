using Application.Exceptions;
using Application.Features.CategoryFeatuure.DTOs;
using Application.Features.CategoryFeatuure.Interfaces;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)=>_categoryService = categoryService;
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAll();
            if (result == null) return NotFound("No Categories Was Retrived");
            return Ok(new ApiResponse<IEnumerable<CategoryDto>>
            {
                TotalRecords = result.Count(),
                Success = true,
                StatusCode = 200,
                Errors = null,
                Data = result,
                Message = "Success"
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result= await _categoryService.GetByid(id);
            if (result is null) return NotFound();
            return Ok(new ApiResponse<CategoryDto>
            {
                Message = "Success",
                Data = result,
                Errors = null,
                StatusCode = 200,
                Success = true,
                TotalRecords = 1
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto createCategoryDTO)
        {
           var createresult =  await _categoryService.Create(createCategoryDTO);
            return Ok(new ApiResponse<Task>
            {
                TotalRecords = 1,
                Success = createresult.Status,
                StatusCode = 200,
                Errors = new List<string> { createresult.MSG },
                Data = Task.CompletedTask
            });
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryDto updateCategoryDTO)
        {
            await _categoryService.Update(updateCategoryDTO);
            return Ok(new ApiResponse<Task>
            {
                Data = Task.CompletedTask,
                Errors = null,
                Message = "Success",
                StatusCode = 200,
                Success = true,
                TotalRecords = 1
            });
        }
        [HttpDelete("{id}")]
        public  async Task<IActionResult> Delete(int id)
        {
            await _categoryService.Delete(id);
            return Ok(new ApiResponse<Task>
            {
                Data = Task.CompletedTask,
                Errors = null,
                Message = "Success",
                StatusCode = 200,
                Success = true,
                TotalRecords = 1
            });
        }
    }
}
