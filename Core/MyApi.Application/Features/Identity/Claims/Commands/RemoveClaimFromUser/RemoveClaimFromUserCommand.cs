using MediatR;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Commands.RemoveClaimFromUser
{
    public record RemoveClaimFromUserCommand(
    Guid UserId,
    string ClaimType,
    string ClaimValue) : IRequest<Result>;
}
