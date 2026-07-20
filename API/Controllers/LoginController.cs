using Application.Features.LoginFeature.DTOs;
using Application.Features.LoginFeature.Interfaces;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class LoginController : ControllerBase
{
    private readonly ILoginService _serviec;
    public LoginController(ILoginService service)
        => _serviec = service;

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _serviec.Login(loginDto);
        if (result.response is null) return NotFound(result.msg);
        return Ok(new ApiResponse<LoginResponse>
        {
            Data = result.response,
            Errors = null,
            Message = result.msg,
            StatusCode = 200,
            Success = true,
            TotalRecords = 1
        });
    }
    [HttpPost("Register")]
    public async Task<IActionResult> Register(RigesterDto rigesterDto)
    {
        var result = await _serviec.Register(rigesterDto);
        if (result.response is null) return NotFound(result.Msg);
        return Ok(new ApiResponse<LoginResponse>
        {
            Data = result.response,
            Errors = null,
            Message = result.Msg,
            StatusCode = 200,
            Success = true,
            TotalRecords = 1
        });
    }
}
