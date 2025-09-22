using MyApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Users.DTOs
{
    public record UserLoginResponseDto 
    {
        public string Username { get; init; } = null!;
        public string Email { get; init; } = null!;
        public IEnumerable<string> Roles { get; init; } = new List<string>();
        public IEnumerable<ClaimDto> Claims { get; init; } = new List<ClaimDto>();
    }
}
