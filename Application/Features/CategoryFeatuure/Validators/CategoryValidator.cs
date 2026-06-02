using Application.Features.CategoryFeatuure.DTOs;
using FluentValidation;
namespace Application.Features.CategoryFeatuure.Validators
{
    public class CategoryValidator:AbstractValidator<CreateCategoryDTO>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .MinimumLength(3);
        }
    }
}
