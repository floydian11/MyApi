using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Users.DTOs
{
    public record LoginDto 
    {
        public string Username { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}
