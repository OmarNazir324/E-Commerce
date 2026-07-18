
using Domain.Entities;

namespace InfraStructure.Identity;

public static class CurrentUser
{
    static AppUser current_user;
    public static void SetCurrent_User(AppUser appUser)
    {
        current_user = appUser;
    }
    public static AppUser GetCurrent_User() => current_user;
}
