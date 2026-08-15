using Application.Exceptions;
using Application.Features.CategoryFeatuure.DTOs;
using Application.Features.CustomerFeature.DTOs;
using Application.Features.CustomerFeature.InterFaces;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
        return Ok(new ApiResponse<IEnumerable<CustomerDto>>
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
        var result = await _customerService.GetById(id);
        if (result == null) return NotFound();
        return Ok(new ApiResponse<CustomerDto>
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
    public async Task<IActionResult> Create(CreateCustomerDto createCustomerDTO)
    {
       var createresult  = await _customerService.Create(createCustomerDTO);
        return Ok(new ApiResponse<Task>
        {
            Data = Task.CompletedTask,
            Errors = null,
            Message = createresult.MSG,
            StatusCode =  createresult.Status ? 200 : 400,
            Success = createresult.Status,
            TotalRecords = 1
        });
    }
    [HttpPut]
    public async Task<IActionResult> Update(UpdateCustomerDto dTO)
    {
        await _customerService.Update(dTO);
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
    public async Task<IActionResult> Delete(int id)
    {
        await _customerService.Delete(id);
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
