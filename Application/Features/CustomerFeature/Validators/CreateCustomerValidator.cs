using Application.Features.CustomerFeature.DTOs;
using FluentValidation;

namespace Application.Features.CustomerFeature.Validators;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerValidator()
    {
        RuleFor(c => c.email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(c => c.Name.Trim() != String.Empty);
        RuleFor(c => c.PhoneNumber).MinimumLength(11);
    }
}
