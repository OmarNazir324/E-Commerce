
namespace Domain.Entities;

public class RefreshToken :Common.CommonEntity
{
    public int U_ID {  get; set; }
    public String U_Token { get; set; }
    public bool Is_Revoked { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool Is_Used { get; set; }
    public AppUser User { get; set; }
}
