using FluentValidation;
using MyApi.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Validators
{
    public class CategoryActiveStatusDtoValidator : AbstractValidator<CategoryActiveStatusDto>
    {
        public CategoryActiveStatusDtoValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty().WithMessage("Geçerli bir Id girilmelidir.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("Aktif/Pasif durumu belirtilmelidir.");
        }
    }
}
