
using Application.Features.LoginFeature.DTOs;
using Application.Responses;
using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Features.LoginFeature.Interfaces;

public interface ILoginService
{   
    Task<LoginResponse> Login(LoginDto loginDto);
    Task<LoginResponse> Register(RigesterDto rigesterDto);
}
