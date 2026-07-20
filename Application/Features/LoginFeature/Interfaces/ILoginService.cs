
using Application.Features.LoginFeature.DTOs;
using Application.Responses;
using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Features.LoginFeature.Interfaces;

public interface ILoginService
{
    Task<(String msg, LoginResponse response)> Login(LoginDto loginDto);
    Task<(String Msg, LoginResponse response)> Register(RigesterDto rigesterDto);
    Task<(bool Exist, AppUser user)> CheckUserExist(String Email);
    String CreateAccessToken(AppUser user);
    String CreateRefreshToken();
    Task<AppUser?> GetUser();
}
