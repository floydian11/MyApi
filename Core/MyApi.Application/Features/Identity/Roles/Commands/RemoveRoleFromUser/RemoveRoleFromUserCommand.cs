using MediatR;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles.Commands.RemoveRoleFromUser
{
    public record RemoveRoleFromUserCommand(
       Guid UserId,
       string RoleName
   ) : IRequest<Result>; // Geriye veri dönmeyeceği için non-generic Result
}
