using Application.Exceptions;
using Application.Features.CustomerFeature.DTOs;
using Application.Features.CustomerFeature.InterFaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            this._customerService= customerService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _customerService.GetAll();
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customerService.GetById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerDTO createCustomerDTO)
        {
            await _customerService.Create(createCustomerDTO);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCustomerDTO dTO)
        {
            await _customerService.Update(dTO);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.Delete(id);
            return Ok();
        }
    }
}
