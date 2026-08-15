using Application.Features.CategoryFeatuure.DTOs;
using FluentValidation;
namespace Application.Features.CategoryFeatuure.Validators;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryDTO>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .MinimumLength(3);
    }
}
