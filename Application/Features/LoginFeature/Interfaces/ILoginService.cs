
using Application.Features.LoginFeature.DTOs;
using Application.Responses;
using Domain.Entities;

namespace Application.Features.LoginFeature.Interfaces;

public interface ILoginService
{
    Task<(String msg, LoginResponse response)> Login(LoginDto loginDto);
    Task<(String Msg, LoginResponse response)> Register(RegisterDto rigesterDto);
    Task<(bool Exist, AppUser user)> CheckUserExist(String Email);

}
