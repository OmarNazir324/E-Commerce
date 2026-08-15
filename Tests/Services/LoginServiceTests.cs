using Application.DataBaseOptions;
using Application.Features.EmailFeature.Interfaces;
using Application.Features.LoginFeature.DTOs;
using Application.Features.LoginFeature.Interfaces;
using Application.Features.LoginFeature.Service;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Tests.Services;

public class LoginServiceTests
{
    private readonly Mock<IMainInterFace<AppUser>> _mock_repo;
    private readonly Mock<IMainInterFace<RefreshToken>> _mock_refreshtoken_repo;
    private readonly Mock<IUnitOfWork> _mock_uow;
    private readonly Mock<IMapper> _mock_mapper;
    private readonly Mock<IPasswordHasher<AppUser>> _mock_passwordhasher;
    private readonly Mock<ILoginService> _mock_service;
    private readonly Mock<IHttpContextAccessor> _mock;
    private readonly Mock<ITokenService> _token_serv;
    private readonly Mock<IEmailService> _email_serv;
    public LoginServiceTests()
    {
        _mock_mapper = new Mock<IMapper>();
        _mock_refreshtoken_repo = new Mock<IMainInterFace<RefreshToken>>();
        _mock_repo = new Mock<IMainInterFace<AppUser>>();
        _mock_uow = new Mock<IUnitOfWork>();
        _mock_passwordhasher = new Mock<IPasswordHasher<AppUser>>();
        _mock_service = new Mock<ILoginService>();
        _mock = new Mock<IHttpContextAccessor>();
        _token_serv = new Mock<ITokenService>();
        _email_serv = new Mock<IEmailService>();
    }
    private LoginService GetLoginService(IOptions<DataBaseOptions> Datbaseoptions)
    {
        return new Application.Features.LoginFeature.Service.LoginService(_mock_passwordhasher.Object, Datbaseoptions, _mock_repo.Object, _mock_uow.Object, _token_serv.Object, _mock_refreshtoken_repo.Object, _email_serv.Object);
    }
    [Fact]
    public async Task Login_ShouldReturnLoginreponse_WhenEmailAndPasswordCorrect()
    {
        #region Arrange
        var logindto = new LoginDto
        {
            Email = "omarr324324@gmail.com",
            Password = "12345678"
        };

        var datbaseoptions = Options.Create(new DataBaseOptions
        {
            RefreshTokenDays = "1",
            Secret = "VerySecretwheniusethetestUnitServiceSoicanKnowitisWorking",
            AccessTokenMinutesForDevelopment = "1",
            AccessTokenMinutes = "0",
            ValidAudience = "UnitTesting",
            ValidIssuer = "UnitTest"
        });
        var user = new AppUser { User_Email = logindto.Email, User_Password = logindto.Password, Id = 1, Name = "Omar" };

        _mock_repo.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<AppUser, bool>>>())).ReturnsAsync(user);

        _mock_mapper.Setup(x => x.Map<AppUser>(logindto)).Returns(user);
        _mock_passwordhasher.Setup(x => x.VerifyHashedPassword(user, user.User_Password, logindto.Password))
            .Returns(PasswordVerificationResult.Success);
        _mock_service.Setup(x => x.CheckUserExist(logindto.Email)).ReturnsAsync((true, user));
        _token_serv.Setup(x => x.CreateAccessToken(It.IsAny<AppUser>())).Returns("TestToken");
        var service = GetLoginService(datbaseoptions);
        #endregion

        #region Act
        var result = await service.Login(logindto);
        #endregion

        #region Assert
        Assert.NotNull(result.response);
        Assert.Equal(user.Id, result.response.User_id);
        Assert.Equal(user.Name, result.response.User_Name);
        Assert.IsType<LoginResponse>(result.response);
        #endregion

    }
    [Fact]
    public async Task Login_ShouldReturnNullResponseAndMSG_WhenWrongPassword()
    {
        #region Arrange
        var logindto = new LoginDto
        {
            Email = "omarr324324@gmail.com",
            Password = "12345678"
        };
        var datbaseoptions = Options.Create(new DataBaseOptions
        {
            RefreshTokenDays = "1",
            Secret = "VerySecretwheniusethetestUnitServiceSoicanKnowitisWorking",
            AccessTokenMinutesForDevelopment = "1",
            AccessTokenMinutes = "0",
            ValidAudience = "UnitTesting",
            ValidIssuer = "UnitTest"
        });
        var user = new AppUser { User_Email = logindto.Email, User_Password = logindto.Password, Id = 1, Name = "Omar" };
        _mock_repo.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<AppUser, bool>>>())).ReturnsAsync(user);
        _mock_mapper.Setup(x => x.Map<AppUser>(logindto)).Returns(user);
        _mock_passwordhasher.Setup(x => x.VerifyHashedPassword(user, user.User_Password, logindto.Password))
            .Returns(PasswordVerificationResult.Failed);
        #endregion
        #region Act
        var service = GetLoginService(datbaseoptions);
        var result = await service.Login(logindto);
        #endregion
        #region Assert
        result.response.Should().BeNull();
        result.msg.Should().Contain("UnCorrect Password");
        #endregion
    }
    [Fact]
    public async Task Login_ShouldReturnNullResponseAndMSG_WhenWrongEmail()
    {
        #region Arrange
        var logindto = new LoginDto
        {
            Email = "omarr324324@gmail.com",
            Password = "12345678"
        };
        var datbaseoptions = Options.Create(new DataBaseOptions
        {
            RefreshTokenDays = "1",
            Secret = "VerySecretwheniusethetestUnitServiceSoicanKnowitisWorking",
            AccessTokenMinutesForDevelopment = "1",
            AccessTokenMinutes = "0",
            ValidAudience = "UnitTesting",
            ValidIssuer = "UnitTest"
        });
        var user = new AppUser { User_Email = logindto.Email, User_Password = logindto.Password, Id = 1, Name = "Omar" };
        _mock_repo.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<AppUser, bool>>>()));
        var service = GetLoginService(datbaseoptions);
        #endregion
        #region Act
        var result = await service.Login(logindto);
        #endregion
        #region Assert
        result.response.Should().BeNull();
        result.msg.Should().NotBeEmpty();
        result.msg.Should().BeSameAs("This Email Doesn't Exist");
        #endregion
    }
    [Fact]
    public async Task Register_ShouldHashPasswordAndCreateUserAndSendWelcomeEmail()
    {
        #region Act
        var registerdto = new RigesterDto
        {
            Email = "omdarr324324@gmail.com",
            Password = "TestPassw0rd",
            UserName = "Omar Nazir"
        };
        var user = new AppUser
        {
            User_Email = registerdto.Email,
            User_Password = registerdto.Password,
            Id = 1,
            Name = "Omar"
        };
        var datbaseoptions = Options.Create(new DataBaseOptions
        {
            RefreshTokenDays = "1",
            Secret = "VerySecretwheniusethetestUnitServiceSoicanKnowitisWorking",
            AccessTokenMinutesForDevelopment = "1",
            AccessTokenMinutes = "0",
            ValidAudience = "UnitTesting",
            ValidIssuer = "UnitTest"
        });
        _mock_repo.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<AppUser, bool>>>())).ReturnsAsync((AppUser?)null);
        _mock_repo.Setup(x => x.Create(It.IsAny<AppUser>())).ReturnsAsync(user);
        _mock_passwordhasher.Setup(x => x.HashPassword(It.IsAny<AppUser>(), registerdto.Password)).Returns("HashedPassword");
        _email_serv.Setup(x => x.SendWelcomeEmail(registerdto.Email)).Returns(Task.CompletedTask);
        var service = GetLoginService(datbaseoptions);
        #endregion
        #region Act
        var result = await service.Register(registerdto);
        #endregion
        #region Assert
        _mock_repo.Verify(x => x.Create(
            It.Is<AppUser>(u =>
                u.User_Email == registerdto.Email &&
                u.Name == registerdto.UserName &&
                u.User_Password == "HashedPassword" &&
                u.Is_Admin == false)),
            Times.Once);
        _mock_passwordhasher.Verify(x => x.HashPassword(It.IsAny<AppUser>(), registerdto.Password), Times.Once);
        _email_serv.Verify(x => x.SendWelcomeEmail(registerdto.Email), Times.Once);
        #endregion
    }

}
