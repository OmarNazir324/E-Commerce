using Application.Features.CategoryFeatuure.DTOs;
using FluentValidation;
namespace Application.Features.CategoryFeatuure.Validators
{
    public class CategoryUpdateValidator:AbstractValidator<UpdateCategoryDTO>
    {
        public CategoryUpdateValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
        }
    }
}
