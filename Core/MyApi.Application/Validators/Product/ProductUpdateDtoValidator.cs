using FluentValidation;
using Microsoft.AspNetCore.Http;
using MyApi.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Validators.Product
{
    public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            // Name boş olamaz (zorunlu kılmak istersek)
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün adı boş olamaz.");

            // Price gönderilmişse pozitif olmalı
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .When(x => x.Price.HasValue)
                .WithMessage("Fiyat negatif olamaz.");

            // ReleaseDate gönderilmişse bugünden ileri olamaz
            RuleFor(x => x.ReleaseDate)
                .LessThanOrEqualTo(DateTime.Now)
                .When(x => x.ReleaseDate.HasValue)
                .WithMessage("Release date bugünden ileri olamaz.");

            // Warranty gönderilmişse pozitif olmalı
            RuleFor(x => x.WarrantyPeriodInMonths)
                .GreaterThanOrEqualTo(0)
                .When(x => x.WarrantyPeriodInMonths.HasValue)
                .WithMessage("Garanti süresi negatif olamaz.");

            // En az bir kategori seçilmişse kontrol et
            RuleFor(x => x.CategoryIds)
                .Must(list => list.Count >= 1)
                .When(x => x.CategoryIds != null && x.CategoryIds.Any())
                .WithMessage("En az bir kategori seçilmelidir.");

            // Ürün resmi varsa boyutu >0 olmalı
            RuleFor(x => x.ProductImage)
                .Must(file => file == null || file.Length > 0)
                .WithMessage("Geçersiz ürün resmi.");

            // Çoklu dosyalar varsa her biri boyutu >0 olmalı
            RuleForEach(x => x.Documents).ChildRules(doc =>
            {
                doc.RuleFor(d => d.Length)
                    .GreaterThan(0)
                    .WithMessage("Geçersiz dosya.");
            });
        }
        }
}
