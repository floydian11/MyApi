using MediatR;
using MyApi.Application.Features.Identity.Roles.DTOs;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles.Commands.AssignRoleToUser
{
   public record AssignRoleToUserCommand(
        Guid UserId,
        string RoleName
    ) : IRequest<Result>; // Geriye veri dönmeyeceği için non-generic Result
}
