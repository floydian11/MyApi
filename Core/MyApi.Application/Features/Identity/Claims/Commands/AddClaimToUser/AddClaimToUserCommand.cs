using MediatR;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Commands.AddClaimToUser
{
    // Bu işlem, UserId, Claim'in Tipi ve Değerini alır. Geriye bir şey dönmez.
    public record AddClaimToUserCommand(
        Guid UserId,
        string ClaimType,
        string ClaimValue) : IRequest<Result>;
}
