
using API;
using Application.Features.LoginFeature.DTOs;
using Application.Responses;
using FluentAssertions;
using InfraStructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Tests.Helpers;
using Xunit;

namespace Tests.Integration;

public class LoginEndpointTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    public LoginEndpointTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    [Fact]
    public async Task Login_ShouldReturn200AndUserExist()
    {
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("Login_");
        string email = $"Login_{Guid.NewGuid()}@gmail.com";
        string password = "Test123@";
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, email, password);
        var response = await _client.PostAsJsonAsync("/api/Login", new LoginDto
        {
            Email = email,
            Password = password
        });
        var responsebody = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responsebody.Should().NotBeNull();
        responsebody.Data.Should().NotBeNull();
        responsebody.Data.AccessToken.Should().NotBeNullOrEmpty();
        responsebody.Data.RefreshToken.Should().NotBeNullOrEmpty();
        responsebody.Data.User_id.Should().NotBe(0);
    }
    [Fact]
    public async Task Login_ShouldReturnUnCorrectPassword()
    {
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("Login_");
        string email = $"Login_{Guid.NewGuid()}@gmail.com";
        string password = "Test123@";
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, email, password);
        var response = await _client.PostAsJsonAsync("/api/Login", new LoginDto
        {
            Email = email,
            Password = password + "78" // Wrong Password
        });
        var responsebody = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        responsebody.Should().Be("UnCorrect Password");
    }
    [Fact]
    public async Task LogOut_ShouldReturn401UnAuthorized()
    {
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("Logout_");
        string email = $"Login_{Guid.NewGuid()}@gmail.com";
        string password = "Test123@";
        var response = await _client.PostAsJsonAsync("/api/Login/LogOut", new LoginDto
        {
            Email = email,
            Password = password
        });
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
    [Fact]
    public async Task Register_ShouldCreateUser()
    {
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("Register_");
        var registerdto = new RigesterDto
        {
            Email = $"omar_{Guid.NewGuid()}@test123.com",
            Password = "Test123@",
            UserName = "Omar_ nazir"
        };
        var response = await _client.PostAsJsonAsync("/api/Login/Register", registerdto);
        var scope = _factory.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<AppdbContext>();
        var context = scope.ServiceProvider.GetRequiredService<AppdbContext>();
        context.AppUsers.Should().HaveCount(1);
        await context.DisposeAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}
