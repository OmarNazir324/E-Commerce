
namespace Application.Responses;

public class LoginResponse
{
    public int User_id { get; set; }
    public String AccessToken { get; set; }
    public String RefreshToken { get; set; }
    public String User_Name { get; set; }
    public String User_Role_Name { get; set; }

}
