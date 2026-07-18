
using Application.Features.LoginFeature.DTOs;
using AutoMapper;
using Domain.Entities;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Tests.Services;

public class LoginServiceTests
{
    private readonly Mock<IMainInterFace<AppUser>> _mock_repo;
    private readonly Mock<IMainInterFace<RefreshToken>> _mock_refreshtoken_repo;
    private readonly Mock<IUnitOfWork> _mock_uow;
    private readonly Mock<IMapper> _mock_mapper;
    private readonly Mock<IPasswordHasher<AppUser>> _mock_passwordhasher;
    private readonly Mock<IServiceProvider> _mock_serviceprovider;
    public LoginServiceTests()
    {
        _mock_mapper = new Mock<IMapper>();
        _mock_refreshtoken_repo = new Mock<IMainInterFace<RefreshToken>>();
        _mock_repo = new Mock<IMainInterFace<AppUser>>();
        _mock_uow = new Mock<IUnitOfWork>();
        _mock_passwordhasher = new Mock<IPasswordHasher<AppUser>>();
        _mock_serviceprovider = new Mock<IServiceProvider>();
    }
    [Fact]
    public async Task Login_ShouldReturnLoginreponse_WhenEmailAndPasswordCorrect()
    {
      var logindto = new LoginDto
      {
        Email = "omarr324324@gmail.com",
        Password ="12345678"
       };
        var user = new AppUser { User_Email = logindto.Email, User_Password = logindto.Password, Id = 1, Name = "Omar" };
        _mock_repo.Setup(x=> x.FindAsync(x=> x.User_Email == logindto.Email)).ReturnsAsync(new List<AppUser> { user });
        _mock_mapper.Setup(x=> x.Map<AppUser>(logindto)).Returns(user); 

       var service = new Application.Features.LoginFeature.Service.LoginService(_mock_passwordhasher.Object,_mock_serviceprovider.Object, _mock_refreshtoken_repo.Object, _mock_repo.Object, _mock_mapper.Object, _mock_uow.Object);
        var result = await service.Login(logindto);
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.User_id);
        Assert.Equal(user.Name, result.User_Name);
    }

}
