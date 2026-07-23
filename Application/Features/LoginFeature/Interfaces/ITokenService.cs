
using Domain.Entities;

namespace Application.Features.LoginFeature.Interfaces;

public interface ITokenService
{
    Task RevokeRefreshToken(string RefreshToken);
    String CreateAccessToken(AppUser user);
    String CreateRefreshToken();
}
