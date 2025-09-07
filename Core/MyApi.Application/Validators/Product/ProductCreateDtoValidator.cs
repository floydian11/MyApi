using FluentValidation;
using MyApi.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Validators.Product
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ürün adı boş olamaz.")
            .MaximumLength(100).WithMessage("Ürün adı 100 karakterden uzun olamaz.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Fiyat 0’dan büyük olmalıdır.");

            RuleFor(x => x.ReleaseDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Release date bugünden ileri olamaz.");

            RuleFor(x => x.CategoryIds)
                .NotEmpty().WithMessage("En az bir kategori seçilmelidir.");

            RuleFor(x => x.ProductImage)
                .Must(file => file != null && file.Length > 0)
                .When(x => x.ProductImage != null)
                .WithMessage("Resim boş olamaz.");

            RuleFor(x => x.Documents)
                .Must(list => list != null && list.Count > 0)
                .When(x => x.Documents != null && x.Documents.Any())
                .WithMessage("En az bir belge eklenmelidir.");
        }
    }
}
