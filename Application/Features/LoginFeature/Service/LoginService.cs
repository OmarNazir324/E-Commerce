using Application.Features.LoginFeature.DTOs;
using Application.Features.LoginFeature.Interfaces;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using InfraStructure.Authentication;
using InfraStructure.Identity;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.LoginFeature.Service;

public class LoginService : ILoginService
{
    private readonly IMainInterFace<RefreshToken> _refreshtoken_repo;
    private readonly IMainInterFace<AppUser> _appuser_repo;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher<AppUser> _passwordhasher;
    private readonly IServiceProvider _serviceProvider;
    public LoginService(IPasswordHasher<AppUser> passwordhasher,IServiceProvider serviceProvider, IMainInterFace<RefreshToken> refreshtoken_repo, IMainInterFace<AppUser> appuser_repo, IMapper mapper, IUnitOfWork uow)
    {
        _refreshtoken_repo = refreshtoken_repo;
        _appuser_repo = appuser_repo;
        _mapper = mapper;
        _uow = uow;
        _serviceProvider = serviceProvider;
        _passwordhasher = passwordhasher;
    }
    private DataBaseOptions.DataBaseOptions Get_DataBaseOptions()
    {
        return _serviceProvider.GetService<IOptions<DataBaseOptions.DataBaseOptions>>()!.Value;
    }
    public async Task<(bool Exist, AppUser user)> CheckUserExist(String Email)
    {
        var result = await _appuser_repo.FindAsync(x => EF.Functions.Like(Email, $"%{x.User_Email}%"));
        var Exist = result.Count() <= 0 && result.FirstOrDefault() is null;
        return (Exist, result.FirstOrDefault()!);
    }
    public async Task<LoginResponse> Register(RigesterDto rigesterDto)
    {
        var ExistUser = await CheckUserExist(rigesterDto.Email);
        if (ExistUser.Exist)
        {
            return await Login(new LoginDto { Email = rigesterDto.Email, Password = rigesterDto.Password });
        }
        else
        {
            await _appuser_repo.Create(new AppUser
            {
                User_Email = rigesterDto.Email,
                User_Password = _passwordhasher.HashPassword(new AppUser(), rigesterDto.Password),
                Name = rigesterDto.UserName,
                Is_Admin = false
            });
            await _uow.SaveChangesAsync();
            return await Login(new LoginDto { Email = rigesterDto.Email, Password = rigesterDto.Password });
        }
    }
    public async Task<LoginResponse> Login(LoginDto loginDto)
    {
        var ExistUser = await CheckUserExist(loginDto.Email);

        if (ExistUser.Exist)
        {
            var user = _mapper.Map<AppUser>(loginDto);
            var orginalPassword = _passwordhasher.VerifyHashedPassword(user, user.User_Password, ExistUser.user.User_Password);
            if (orginalPassword == PasswordVerificationResult.Success)
            {
                var accesstoken = CreateAccessToken(user);
                var refreshtoken = CreateRefreshToken();
                await _refreshtoken_repo.Create(new RefreshToken
                {
                    ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToDouble(Get_DataBaseOptions().RefreshTokenDays)),
                    U_Token = refreshtoken,
                    U_ID = user.Id,
                    User = user,
                });
                await _uow.SaveChangesAsync();
                CurrentUser.SetCurrent_User(user);
                return new LoginResponse
                {
                    AccessToken = accesstoken.ToString(),
                    RefreshToken = refreshtoken,
                    User_id = user.Id,
                    User_Name = user.Name,
                    User_Role_Name = user.UserRoles.FirstOrDefault().ToString()
                };
            }
            return null;
        }
        return null;
    }
    private JwtSecurityToken CreateAccessToken(AppUser user)
    {
        var datbaseoptions = Get_DataBaseOptions();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, HashBase.Encrypt(user.Id.ToString())),
            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        foreach (var userole in user.UserRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userole.ToString()));
        }
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(datbaseoptions.Secret));
        var expires = DateTime.UtcNow.AddMinutes(
            Convert.ToDouble(user.UserRoles.Contains(User_Roles.Admin) && user.UserRoles.Contains(User_Roles.Developer) ? datbaseoptions.AccessTokenMinutesForDevelopment : datbaseoptions.AccessTokenMinutes));
        var accesstoken = new JwtSecurityToken(issuer: datbaseoptions.ValidIssuer, audience: datbaseoptions.ValidAudience, expires: expires, claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
        return accesstoken;
    }
    private String CreateRefreshToken()
    {
        var randomBytes = new byte[64];

        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }
}
