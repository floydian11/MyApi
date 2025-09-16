using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Users.Commands.RegisterUser
{
    // AbstractValidator<T>'den miras alır ve hangi komutu doğrulayacağını belirtir.
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        // Doğrulama kuralları constructor içinde tanımlanır.
        public RegisterUserCommandValidator()
        {
            // Username alanı boş olamaz ve maksimum 50 karakter olabilir.
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
                .MaximumLength(50).WithMessage("Kullanıcı adı 50 karakterden uzun olamaz.");

            // Email alanı boş olamaz ve geçerli bir email formatında olmalıdır.
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta adresi boş olamaz.")
                .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi girin.");
            //identity ayarları ile uyumlu olmalı. burada bu kontrolü yaparak servera gitmeden hatayı yakalamış oluyoruz.
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz.")
                .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
                .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
                .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
                .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az bir özel karakter (örn: !?*.) içermelidir.");

            // Diğer alanlar için de kurallar eklenebilir...
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.TCKN).NotEmpty().Length(11);
        }
    }
}
