using Application.Features.OrderFeature.DTOs;
using Application.Features.OrderFeature.InterFace;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)=> this._orderService = orderService;
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAll();
            if (result is null) return (IActionResult)Results.NotFound("There IS No Orders Retrieved");
            return Ok(result);
        }
        [HttpGet("{orderid:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _orderService.GetById(id);
            if (result is null) return NotFound("There is no orders with this ID");
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDTO createOrderDTO)
        {
            await _orderService.Create(createOrderDTO);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateOrderDTO updateOrderDTO)
        {
            await _orderService.Update(updateOrderDTO);
            return Ok();
        }
        [HttpDelete("{orderid:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.Delete(id);
            return Ok();
        }
    }
}
