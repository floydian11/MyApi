using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            // Güncellenecek rolün ID'si boş olamaz.
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Rol ID'si boş olamaz.");

            // Rolün yeni adı boş olamaz.
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Rol adı boş olamaz.")
                .MaximumLength(50).WithMessage("Rol adı 50 karakterden uzun olamaz.");

            // Açıklama alanı için de bir uzunluk kısıtlaması koymak iyi bir pratiktir.
            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Açıklama 200 karakterden uzun olamaz.");
        }
    }
}
