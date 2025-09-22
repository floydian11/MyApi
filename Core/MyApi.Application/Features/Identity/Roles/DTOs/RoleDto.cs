using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles.DTOs
{
    public record RoleDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty; // Bu, "Product.Read" gibi kod olacak
        public string? Description { get; init; } = string.Empty; // Bu, "Ürün Okuma Yetkisi" gibi görünen ad olacak
    }
}
