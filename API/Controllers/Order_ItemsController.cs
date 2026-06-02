using Application.Features.Order_ItemsFeature.DTOs;
using Application.Features.Order_ItemsFeature.InterFace;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Order_ItemsController : ControllerBase
{
    private readonly IOrder_ItemsService _order_ItemsService;
    public Order_ItemsController(IOrder_ItemsService order_ItemsService)
        => this._order_ItemsService= order_ItemsService;
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _order_ItemsService.GetAll();
        if (result is null) return NotFound();
        return Ok(result);
    }
    [HttpGet("{Order_Items_ID:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _order_ItemsService.GetById(id);
        if(result is null) return NotFound();
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrder_itemsDTO createOrder_ItemsDTO)
    {
        await _order_ItemsService.Create(createOrder_ItemsDTO);
        return Ok();
    }
    [HttpPut]
    public async Task<IActionResult> Update(UpdateOrder_ItemsDTO updateOrder_ItemsDTO)
    {
        await _order_ItemsService.Update(updateOrder_ItemsDTO);
        return Ok();
    }
    [HttpDelete("{Order_Items_Id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _order_ItemsService.Delete(id);
        return Ok();
    }
}
