using FluentValidation;
using MyApi.Application.DTOs.ARServices.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Validators
{
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori adı boş olamaz.")
                .MaximumLength(50).WithMessage("Kategori adı 50 karakteri geçemez.");
        }
    }
}
