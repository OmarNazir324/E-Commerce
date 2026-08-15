using Application.Features.LoginFeature.DTOs;
using Application.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;
namespace Tests.Helpers;

public static class LoginHelperForIntegrationTest
{
    public static async Task AuthenticateAsync(HttpClient _client, string email, string password)
    {
        var RigesterDto = new RegisterDto
        {
            Email = email,
            Password = password,
            UserName = "Omar Nazir"
        };
        // Login
        var loginResponse = await _client.PostAsJsonAsync("/api/Login/Register", RigesterDto);

        var token = await loginResponse.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.Data.AccessToken
            );
    }

}
