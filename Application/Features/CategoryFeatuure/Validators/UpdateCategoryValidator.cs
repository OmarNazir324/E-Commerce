using Application.Features.CategoryFeatuure.DTOs;
using FluentValidation;
namespace Application.Features.CategoryFeatuure.Validators;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDTO>
{
    public UpdateCategoryValidator() 
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
    }
}
