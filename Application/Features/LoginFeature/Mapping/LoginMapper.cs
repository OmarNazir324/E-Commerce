
using Application.Features.LoginFeature.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.LoginFeature.Mapping;

public class LoginMapper:Profile
{
    public LoginMapper()
    {
        CreateMap<AppUser, LoginDto>().ReverseMap();
    }
}
