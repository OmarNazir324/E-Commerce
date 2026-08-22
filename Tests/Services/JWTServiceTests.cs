using Application.DataBaseOptions;
using Application.Features.EmailFeature.Interfaces;
using Application.Features.LoginFeature.DTOs;
using Application.Features.LoginFeature.Interfaces;
using Application.Features.LoginFeature.Service;
using Domain.Entities;
using FluentAssertions;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Tests.Services;

public class JWTServiceTests
{
    private readonly Mock<IGenericRepository<RefreshToken>> _token_repo;
    private readonly Mock<ITokenService> _token_serv;
    private readonly Mock<IPasswordHasher<AppUser>> _password_hasher;
    private readonly Mock<IGenericRepository<AppUser>> _appuser_repo;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IEmailService> _email_serv;
    public JWTServiceTests()
    {
        _token_repo = new Mock<IGenericRepository<RefreshToken>>();
        _token_serv = new Mock<ITokenService>();
        _password_hasher = new Mock<IPasswordHasher<AppUser>>();
        _appuser_repo = new Mock<IGenericRepository<AppUser>>();
        _uow = new Mock<IUnitOfWork>();
        _email_serv = new Mock<IEmailService>();       
    }
    LoginService GetLoginService(IOptions<DataBaseOptions> databaseoptions)
        => new LoginService(_password_hasher.Object, databaseoptions, _appuser_repo.Object, _uow.Object, _token_serv.Object, _token_repo.Object,_email_serv.Object);
    [Fact]
    public async Task CreateAccessToken_ShouldBeSameasTheloginValue()
    {
        #region Arrange
        var loginDto = new LoginDto
        {
            Email = "Omar324324@gmail.com",
            Password = "TestPassword"
        };
        var user = new AppUser
        {
            Is_Admin = true,
            Id = 1,
            Name = "Omar",
            User_Email = "omarr324324@gmail.com",
            User_Password = "TestPassw0rd"
        };
        var database_options = Options.Create(new DataBaseOptions
        {
        });
        _token_serv.Setup(x => x.CreateAccessToken(user))
            .Returns("FakeAccessToken");
        _token_serv.Setup(x => x.CreateRefreshToken())
            .Returns("FakeRefreshToken");
        _password_hasher.Setup(x => x.VerifyHashedPassword(user, user.User_Password, loginDto.Password))
            .Returns(PasswordVerificationResult.Success);
        _appuser_repo.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<AppUser, bool>>>())).ReturnsAsync(user);
        var loginservice = GetLoginService(database_options);
        #endregion
        #region Act
        var result = await loginservice.Login(loginDto);
        #endregion
        #region Assert
        result.response.AccessToken.Should().BeSameAs("FakeAccessToken");
        result.response.RefreshToken.Should().BeSameAs("FakeRefreshToken");
        result.msg.Should().BeSameAs("Login Success");
        #endregion
    }
}
