using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Users.DTOs
{    // Bu record, bir kullanıcının rollerini ve claim'lerini bir arada taşır.

    public record UserRolesAndClaimsDto
    {
        public IEnumerable<string> Roles { get; init; } = new List<string>();
        public IEnumerable<Claim> Claims { get; init; } = new List<Claim>();
    }
}
