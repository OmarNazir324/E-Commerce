
using Domain.Entities;

namespace Application.Features.LoginFeature.Interfaces;

public interface ICurrentUserService
{
    Task<AppUser?> GetUser();

}
