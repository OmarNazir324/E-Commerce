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
    private readonly IGenericRepository<AppUser> _appuserRepository;
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private readonly DataBaseOptions.DataBaseOptions _databaseoptions;
    private readonly ITokenService _tokenService;
    private readonly IGenericRepository<RefreshToken> _tokenRepository;
    private readonly IEmailService _emailService;
    public LoginService(IPasswordHasher<AppUser> passwordhasher, IOptions<DataBaseOptions.DataBaseOptions> _options,
         IGenericRepository<AppUser> appuser_repo, IUnitOfWork uow, ITokenService token_serv, IGenericRepository<RefreshToken> token_repo, IEmailService email_serv)
    {
        _appuserRepository = appuser_repo;
        _uow = uow;
        _databaseoptions = _options.Value;
        _passwordHasher = passwordhasher;
        _tokenService = token_serv;
        _tokenRepository = token_repo;
        _emailService = email_serv;
    }
    public async Task<(bool Exist, AppUser? user)> CheckUserExist(string email)
    {
        var user = await _appuserRepository.FirstOrDefaultAsync(x => x.User_Email == email);
        return (user != null, user);
    }


    public async Task<(String Msg, LoginResponse response)> Register(RegisterDto registerDto)
    {
        var existingUser = await CheckUserExist(registerDto.Email);
        if (existingUser.Exist)
        {
            return await LoginOperation(existingUser.user, registerDto.Password);
        }
        else
        {
          var appuser =  await _appuserRepository.AddAsync(new AppUser
            {
                User_Email = registerDto.Email,
                User_Password = _passwordHasher.HashPassword(new AppUser(), registerDto.Password),
                Name = registerDto.UserName,
                Is_Admin = false
            });
            await _uow.SaveChangesAsync();
            var result = await LoginOperation(appuser, registerDto.Password);
            await _emailService.SendWelcomeEmail(registerDto.Email);
            return result;
        }
    }
    public async Task<(String msg, LoginResponse response)> Login(LoginDto loginDto)
    {
        var existingUser = await CheckUserExist(loginDto.Email);

        if (existingUser.Exist)
        {
            return await LoginOperation(existingUser.user, loginDto.Password);
        }
        return ("This Email Doesn't Exist", null)!;
    }
    private async Task<(String msg, LoginResponse response)> LoginOperation(AppUser user,string password)
    {
        var orginalPassword = _passwordHasher.VerifyHashedPassword(user, user.User_Password, password);
        if (orginalPassword == PasswordVerificationResult.Success)
        {
            var accesstoken = _tokenService.CreateAccessToken(user);
            var refreshtoken = _tokenService.CreateRefreshToken();
            await _tokenRepository.AddAsync(new RefreshToken
            {
                ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToDouble(_databaseoptions.RefreshTokenDays)),
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
