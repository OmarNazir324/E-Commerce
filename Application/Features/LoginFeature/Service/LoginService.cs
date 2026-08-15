using Application.Features.EmailFeature.Interfaces;
using Application.Features.LoginFeature.DTOs;
using Application.Features.LoginFeature.Interfaces;
using Application.Responses;
using Domain.Entities;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Formats.Asn1;

namespace Application.Features.LoginFeature.Service;

public class LoginService : ILoginService
{
    private readonly IMainInterFace<AppUser> _appuser_repo;
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher<AppUser> _passwordhasher;
    private readonly DataBaseOptions.DataBaseOptions _database_options;
    private readonly ITokenService _token_serv;
    private readonly IMainInterFace<RefreshToken> _token_repo;
    private readonly IEmailService _email_serv;
    public LoginService(IPasswordHasher<AppUser> passwordhasher, IOptions<DataBaseOptions.DataBaseOptions> _options,
         IMainInterFace<AppUser> appuser_repo, IUnitOfWork uow, ITokenService token_serv, IMainInterFace<RefreshToken> token_repo, IEmailService email_serv)
    {
        _appuser_repo = appuser_repo;
        _uow = uow;
        _database_options = _options.Value;
        _passwordhasher = passwordhasher;
        _token_serv = token_serv;
        _token_repo = token_repo;
        _email_serv = email_serv;
    }
    public async Task<(bool Exist, AppUser? user)> CheckUserExist(string email)
    {
        var user = await _appuser_repo.FirstOrDefaultAsync(x => x.User_Email == email);
        return (user != null, user);
    }


    public async Task<(String Msg, LoginResponse response)> Register(RigesterDto rigesterDto)
    {
        var ExistUser = await CheckUserExist(rigesterDto.Email);
        if (ExistUser.Exist)
        {
            return await LoginOperation(ExistUser.user, rigesterDto.Password);
        }
        else
        {
          var appuser =  await _appuser_repo.Create(new AppUser
            {
                User_Email = rigesterDto.Email,
                User_Password = _passwordhasher.HashPassword(new AppUser(), rigesterDto.Password),
                Name = rigesterDto.UserName,
                Is_Admin = false
            });
            await _uow.SaveChangesAsync();
            var result = await LoginOperation(appuser, rigesterDto.Password);
            await _email_serv.SendWelcomeEmail(rigesterDto.Email);
            return result;
        }
    }
    public async Task<(String msg, LoginResponse response)> Login(LoginDto loginDto)
    {
        var ExistUser = await CheckUserExist(loginDto.Email);

        if (ExistUser.Exist)
        {
            return await LoginOperation(ExistUser.user, loginDto.Password);
        }
        return ("This Email Doesn't Exist", null)!;
    }
    private async Task<(String msg, LoginResponse response)> LoginOperation(AppUser user,string password)
    {
        var orginalPassword = _passwordhasher.VerifyHashedPassword(user, user.User_Password, password);
        if (orginalPassword == PasswordVerificationResult.Success)
        {
            var accesstoken = _token_serv.CreateAccessToken(user);
            var refreshtoken = _token_serv.CreateRefreshToken();
            await _token_repo.Create(new RefreshToken
            {
                ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToDouble(_database_options.RefreshTokenDays)),
                U_Token = refreshtoken,
                U_ID = user.Id
            });
            await _uow.SaveChangesAsync();
            CurrentUser.SetCurrent_User(user);
            return ("Login Success", new LoginResponse
            {
                AccessToken = accesstoken,
                RefreshToken = refreshtoken,
                User_id = user.Id,
                User_Name = user.Name,
                User_Role_Name = user.UserRoles.FirstOrDefault().ToString()
            });
        }
        return ("UnCorrect Password", null)!;
    }
}
