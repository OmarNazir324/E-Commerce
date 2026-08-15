using Application.Exceptions;
using Application.Features.CategoryFeatuure.DTOs;
using Application.Features.Product.DTOs;
using Application.Features.Product.Interfaces;
using Application.Features.ProductFeature.DTOs;
using Application.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)=>_productService = productService;
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetProducts();
            if (result == null) return NotFound();
            return Ok(new ApiResponse<IEnumerable<ProductDTO>>
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
            var result = await _productService.GetProductById(id);
            if (result == null) return NotFound();
            return Ok(new ApiResponse<ProductDTO>
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
        public async Task<IActionResult> Create(CreateProductDTO createProductDTO)
        {
           var createresult =  await _productService.Create(createProductDTO);
            return Ok(new ApiResponse<Product>
            {
                Data = createresult.entity,
                Errors = null,
                Message = createresult.MSG,
                StatusCode = 200,
                Success = createresult.Status,
                TotalRecords = 1
            });
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductDTO updateProductDTO)
        {
            await _productService.Update(updateProductDTO);
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
            await _productService.Delete(id);
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
