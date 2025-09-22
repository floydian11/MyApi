using MediatR;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Commands.DeleteClaimTemplate
{
    public record DeleteClaimTemplateCommand(Guid Id) : IRequest<Result>;


}
