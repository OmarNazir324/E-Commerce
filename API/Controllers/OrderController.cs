using Application.Features.OrderFeature.DTOs;
using Application.Features.OrderFeature.InterFace;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService) => this._orderService = orderService;
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAll();
            if (result is null) return (IActionResult)Results.NotFound("There IS No Orders Retrieved");
            return Ok(new ApiResponse<IEnumerable<OrderDto>>
            {
                TotalRecords = result.Count(),
                Success = true,
                StatusCode = 200,
                Errors = null,
                Data = result,
                Message = "Success"
            });
        }
        [HttpGet("{orderid:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _orderService.GetById(id);
            if (result is null) return NotFound("There is no orders with this ID");
            return Ok(new ApiResponse<OrderDto>
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
        public async Task<IActionResult> Create(CreateOrderDto createOrderDTO)
        {
            var createresult = await _orderService.Create(createOrderDTO);
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
        public async Task<IActionResult> Update(UpdateOrderDto updateOrderDTO)
        {
            await _orderService.Update(updateOrderDTO);
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
        [HttpDelete("{orderid:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.Delete(id);
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
