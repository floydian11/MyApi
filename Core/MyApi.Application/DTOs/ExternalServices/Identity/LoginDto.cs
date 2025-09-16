using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.ExternalServices.Account
{
    public class LoginDto : IDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
