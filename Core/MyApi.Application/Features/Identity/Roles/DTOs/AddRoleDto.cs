using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles.DTOs
{
    public record AddRoleDto(string Name, string? Description);
}
