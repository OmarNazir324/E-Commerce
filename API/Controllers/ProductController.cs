using Application.Exceptions;
using Application.Features.Product.Interfaces;
using Application.Features.ProductFeature.DTOs;
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
            if (result == null) throw new NotFoundException("No Products Was Retrived");
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result= await _productService.GetProductById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDTO createProductDTO)
        {
            await _productService.Create(createProductDTO);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductDTO updateProductDTO)
        {
            await _productService.Update(updateProductDTO);
            return Ok();
        }
        [HttpDelete("{id}")]
        public  async Task<IActionResult> Delete(int id)
        {
            await _productService.Delete(id);
            return Ok();
        }
    }
}
