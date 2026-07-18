
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.LoginFeature.DTOs;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public String Email { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$", ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character.")]
    public String Password { get; set; }
}
