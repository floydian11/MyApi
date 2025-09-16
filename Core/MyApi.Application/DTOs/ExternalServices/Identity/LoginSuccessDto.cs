using MyApi.Application.DTOs.ExternalServices.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.ExternalServices.Identity
{
    public class LoginSuccessDto : IDto
    {
        public TokenResponseDto Tokens { get; set; } = null!;
        public UserLoginResponseDto User { get; set; } = null!;
    }
}
