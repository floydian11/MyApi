using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator() 
        {
            // Username alanı boş olamaz ve maksimum 50 karakter olabilir.
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
                .MaximumLength(50).WithMessage("Kullanıcı adı 50 karakterden uzun olamaz.");

            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("E-posta adresi boş olamaz.")

               // .EmailAddress() metodunu kaldırıyoruz ve formatı tamamen
               // kendi özel Regex'imiz ile kontrol ediyoruz.
               .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
               .WithMessage("Lütfen 'isim@alanadı.uzantı' formatında geçerli bir e-posta adresi girin.");
           

            // Diğer alanlar için de kurallar eklenebilir...
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.TCKN).NotEmpty().Length(11);
        }
    }
}
