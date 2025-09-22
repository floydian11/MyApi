using FluentValidation;
using MyApi.Application.Features.Identity.Users.Commands.RegisterUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles.Commands.AddRole
{
    public class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
    {
        public AddRoleCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Rol adı boş olamaz.")
                .MaximumLength(50).WithMessage("Kullanıcı adı 50 karakterden uzun olamaz.");

            RuleFor(x => x.Description)
               .NotEmpty().WithMessage("Rol açıklaması boş olamaz.")
               .MaximumLength(200).WithMessage("Rol açıklaması 200 karakterden uzun olamaz.");
        }
        
    }
}
