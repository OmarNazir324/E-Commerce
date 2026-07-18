
using Application.Features.LoginFeature.DTOs;
using FluentValidation;

namespace Application.Features.LoginFeature.Validators;

public class LoginValidator:AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email.Contains("@"));
    }
}
