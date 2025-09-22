using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.DTOs
{
    public record RemoveClaimFromUserDto
    {
        /// <summary>
        /// Kaldırılacak yetkinin tipi. Örn: "permission"
        /// </summary>
        public string ClaimType { get; init; } = string.Empty;

        /// <summary>
        /// Kaldırılacak yetkinin değeri. Örn: "products.delete"
        /// </summary>
        public string ClaimValue { get; init; } = string.Empty;
    }
}
