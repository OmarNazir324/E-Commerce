using Application.Features.ProductFeature.DTOs;
using FluentValidation;

namespace Application.Features.Product.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}
