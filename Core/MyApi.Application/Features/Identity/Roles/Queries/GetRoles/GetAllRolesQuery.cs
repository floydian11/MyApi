using MediatR;
using MyApi.Application.Features.Identity.Roles.DTOs;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles.Queries.GetRoles
{
    namespace MyApi.Application.Features.Identity.Roles.Queries.GetAllRoles
    {
        // Bu sorgu herhangi bir parametre almaz.
        // Başarılı olduğunda, bir RoleDto listesi döneceğini belirtir.
        public record GetAllRolesQuery() : IRequest<Result<List<RoleDto>>>;
    }
}
