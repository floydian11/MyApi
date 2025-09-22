using FluentValidation;
using MyApi.Application.Features.Identity.Claims.Commands.CreateClaimTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Commands.UpdateClaimTemplate
{
    public class UpdateClaimTemplateCommandValidator : AbstractValidator<UpdateClaimTemplateCommand>
    {
        public UpdateClaimTemplateCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id boş geçilemez.");
            RuleFor(x => x.Type).NotEmpty().MaximumLength(100).WithMessage("Yetki tipi boş geçilemez veya 100 karakterden az olamaz.");
            RuleFor(x => x.Value).NotEmpty().MaximumLength(100).WithMessage("Yetki değeri boş geçilemez veya 100 karakterden az olamaz.");
            RuleFor(x => x.Description).NotEmpty().MaximumLength(250).WithMessage("Yetki açıklaması boş geçilemez veya 100 karakterden az olamaz.");
        }
    }
}
