
using Application.Features.LoginFeature.Interfaces;
using Domain.Entities;
using InfraStructure.Authentication;
using InfraStructure.Identity;
using InfraStructure.Repositories.Generic;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Features.LoginFeature.Service;

public class CurrentUserService:ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMainInterFace<AppUser> _appuser_repo;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor,IMainInterFace<AppUser> appuser_repo)
    {
        _httpContextAccessor = httpContextAccessor;
        _appuser_repo = appuser_repo;
    }

    public async Task<AppUser?> GetUser()
    {
        var currentuser = CurrentUser.GetCurrent_User();
        if (currentuser != null) return currentuser;
        var encryptedId = _httpContextAccessor.HttpContext!.User
            .FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var u_id = int.Parse(HashBase.Decrypt(encryptedId));
        var user = await _appuser_repo.GetByID(u_id);
        CurrentUser.SetCurrent_User(user!);
        return user;
    }
    
}
