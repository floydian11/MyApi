using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Commands.CreateClaimTemplate
{
    public class CreateClaimTemplateCommandValidator : AbstractValidator<CreateClaimTemplateCommand>
    {
        public CreateClaimTemplateCommandValidator()
        {
            RuleFor(x => x.Type).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Value).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(250);
        }
    }
}
