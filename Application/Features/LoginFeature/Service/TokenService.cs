
using Application.DataBaseOptions;
using Application.Features.LoginFeature.Interfaces;
using Domain.Entities;
using Domain.Enums;
using InfraStructure.Authentication;
using InfraStructure.Repositories.Generic;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.LoginFeature.Service;

public class TokenService :ITokenService
{
    private readonly DataBaseOptions.DataBaseOptions _database_options;
    private readonly IMainInterFace<RefreshToken> _refresh_repo;
    public TokenService(IOptions<DataBaseOptions.DataBaseOptions> database_options,IMainInterFace<RefreshToken> refresh_repo)
    {
        _database_options = database_options.Value;
        _refresh_repo = refresh_repo;
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
    public async Task RevokeRefreshToken(string RefreshToken)
    {
        var refreshToken = await _refresh_repo.FirstOrDefaultAsync(x => x.U_Token == RefreshToken);

        if (refreshToken == null) return;

        refreshToken.Is_Revoked =
                true;
        await _refresh_repo.Update(refreshToken);
    }
}
