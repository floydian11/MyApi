using MediatR;
using MyApi.Application.Features.Identity.Roles.DTOs;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles.Commands.AddRole
{
    public record AddRoleCommand(
        string Name,
        string? Description
        ) : IRequest<Result<RoleDto>>;
}
