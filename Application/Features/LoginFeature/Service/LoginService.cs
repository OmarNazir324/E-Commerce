using Application.Features.Email.Interfaces;
using Application.Features.LoginFeature.DTOs;
using Application.Features.LoginFeature.Interfaces;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using InfraStructure.Identity;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
         IMainInterFace<AppUser> appuser_repo, IUnitOfWork uow, ITokenService token_serv, IMainInterFace<RefreshToken> token_repo,IEmailService email_serv)
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
            await _email_serv.SendWelcomeEmail(rigesterDto.Email);
            return result;
        }
    }
    public async Task<(String msg, LoginResponse response)> Login(LoginDto loginDto)
    {
        var ExistUser = await CheckUserExist(loginDto.Email);

        if (ExistUser.Exist)
        {
            var orginalPassword = _passwordhasher.VerifyHashedPassword(ExistUser.user, ExistUser.user.User_Password, loginDto.Password);
            if (orginalPassword == PasswordVerificationResult.Success)
            {
                var accesstoken = _token_serv.CreateAccessToken(ExistUser.user);
                var refreshtoken = _token_serv.CreateRefreshToken();
                await _token_repo.Create(new RefreshToken
                {
                    ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToDouble(_database_options.RefreshTokenDays)),
                    U_Token = refreshtoken,
                    U_ID = ExistUser.user.Id
                });
                await _uow.SaveChangesAsync();
                CurrentUser.SetCurrent_User(ExistUser.user);
                return ("Login Success", new LoginResponse
                {
                    AccessToken = accesstoken,
                    RefreshToken = refreshtoken,
                    User_id = ExistUser.user.Id,
                    User_Name = ExistUser.user.Name,
                    User_Role_Name = ExistUser.user.UserRoles.FirstOrDefault().ToString()
                });
            }
            return ("UnCorrect Password", null)!;
        }
        return ("This Email Doesn't Exist", null)!;
    }



}
