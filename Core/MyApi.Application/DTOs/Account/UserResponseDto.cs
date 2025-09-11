using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.Account
{
    public class UserResponseDto : IDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public IEnumerable<ClaimDto> Claims { get; set; } = new List<ClaimDto>();
    }
}
