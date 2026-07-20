using Domain.Enums;
namespace Domain.Entities;

public class AppUser:Common.CommonEntity
{
    public string User_Email { get; set; }
    public string User_Password { get; set; }
    public bool Is_Admin { get; set; } = false;
    public List<int> UserRoles { get; set; } = new List<int>() { ((int)User_Roles.Admin) };
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}   
