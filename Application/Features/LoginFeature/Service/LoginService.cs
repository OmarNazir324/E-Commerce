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
using Microsoft.AspNetCore.Http;
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
    private readonly DataBaseOptions.DataBaseOptions _database_options;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public LoginService(IHttpContextAccessor httpContextAccessor,IPasswordHasher<AppUser> passwordhasher,IOptions<DataBaseOptions.DataBaseOptions> _options, IMainInterFace<RefreshToken> refreshtoken_repo, IMainInterFace<AppUser> appuser_repo, IMapper mapper, IUnitOfWork uow)
    {
        _refreshtoken_repo = refreshtoken_repo;
        _appuser_repo = appuser_repo;
        _mapper = mapper;
        _uow = uow;
        _database_options = _options.Value;
        _passwordhasher = passwordhasher;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<(bool Exist, AppUser? user)> CheckUserExist(string email)
    {
        var user = await _appuser_repo.GetCurrentContext.Set<AppUser>()
            .Where(x => x.User_Email == email)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        

        return (user != null, user);
    }
    public async Task<(String Msg,LoginResponse response)> Register(RigesterDto rigesterDto)
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
            var result = await Login(new LoginDto { Email = rigesterDto.Email, Password = rigesterDto.Password });
            return result;
        }
    }
    public async Task<(String msg,LoginResponse response)> Login(LoginDto loginDto)
    {
        var ExistUser = await CheckUserExist(loginDto.Email);

        if (ExistUser.Exist)
        {
            var orginalPassword = _passwordhasher.VerifyHashedPassword(ExistUser.user, ExistUser.user.User_Password,loginDto.Password);
            if (orginalPassword == PasswordVerificationResult.Success)
            {
                var accesstoken = CreateAccessToken(ExistUser.user);
                var refreshtoken = CreateRefreshToken();
                await _refreshtoken_repo.Create(new RefreshToken
                {
                    ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToDouble(_database_options.RefreshTokenDays)),
                    U_Token = refreshtoken,
                    U_ID = ExistUser.user.Id
                });
                await _uow.SaveChangesAsync();
                CurrentUser.SetCurrent_User(ExistUser.user);
                return ( "Login Success" ,new LoginResponse
                {
                    AccessToken = accesstoken,
                    RefreshToken = refreshtoken,
                    User_id = ExistUser.user.Id,
                    User_Name = ExistUser.user.Name,
                    User_Role_Name = ExistUser.user.UserRoles.FirstOrDefault().ToString()
                });
            }
            return ("UnCorrect Password" , null)!;
        }
        return ("This Email Doesn't Exist" ,null)!;
    }
    public String CreateAccessToken(AppUser user)
    {
       
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, HashBase.Encrypt(user.Id.ToString())),
            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        foreach (var userole in user.UserRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userole.ToString()));
        }
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_database_options.Secret));
        var expires = DateTime.UtcNow.AddMinutes(
            Convert.ToDouble(user.UserRoles.Contains((int)User_Roles.Admin) && user.UserRoles.Contains((int)User_Roles.Developer) ? _database_options.AccessTokenMinutesForDevelopment : _database_options.AccessTokenMinutes));
        var accesstoken = new JwtSecurityToken(issuer: _database_options.ValidIssuer, audience: _database_options.ValidAudience, expires: expires, claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(accesstoken);
    }
    public String CreateRefreshToken()
    {
        var randomBytes = new byte[64];

        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
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
