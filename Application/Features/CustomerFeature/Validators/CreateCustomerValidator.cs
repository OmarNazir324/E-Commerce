using Application.Features.CustomerFeature.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerFeature.Validators
{
    public class CreateCustomerValidator:AbstractValidator<CreateCustomerDTO>
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
}
