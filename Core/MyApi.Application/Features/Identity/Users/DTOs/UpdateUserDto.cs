using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Users.DTOs
{
    public record UpdateUserDto
    {
        public string Username { get; init; } = string.Empty;
        public string? Email { get; init; } = string.Empty;
        public string? FirstName { get; init; } = string.Empty;
        public string? LastName { get; init; } = string.Empty;
        public string? TCKN { get; init; } = string.Empty;  // Opsiyonel, hash servisi ile işlenecek
        public bool? IsActive { get; init; } = false; // Opsiyonel, admin update için

    }
}
