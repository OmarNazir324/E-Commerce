using Domain.Enums;
namespace Domain.Entities;

public class AppUser:Common.CommonEntity
{
    public string User_Email { get; set; }
    public string User_Password { get; set; }
    public bool Is_Admin { get; set; } = false;
    public List<User_Roles> UserRoles { get; set; } = new List<User_Roles>() { User_Roles.User };
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}   
