
using Application.Features.LoginFeature.DTOs;
using Application.Responses;
using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Features.LoginFeature.Interfaces;

public interface ILoginService
{
    Task<bool> CheckUserExist(LoginDto loginDto);
    Task<LoginResponse> Login(LoginDto loginDto);
    JwtSecurityToken CreateAccessToken(AppUser user);
    String CreateRefreshToken();
    Task<LoginDto> Register(AppUser appUser);
}
