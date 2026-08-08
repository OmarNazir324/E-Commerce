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
    private readonly ILoginService _login_serv;
    private readonly ITokenService _token_serv;
    public LoginController(ILoginService login_serv,ITokenService token_serv)
    {
        _login_serv = login_serv;
        _token_serv = token_serv;
    }
    /*
     new ApiResponse<String>
        {
            Data = null,
            Errors = new List<string> { result.msg },
            Message = result.msg,
            StatusCode = 404,
            Success = false,
            TotalRecords = 0
        }
    */
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _login_serv.Login(loginDto);
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
        var result = await _login_serv.Register(rigesterDto);
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
    [Authorize]
    [HttpPost("LogOut")]
    public async Task<IActionResult> Logout(
     String rereshtoken)
    {
        await _token_serv.RevokeRefreshToken(rereshtoken);
        return Ok(Task.CompletedTask);
    }
}
