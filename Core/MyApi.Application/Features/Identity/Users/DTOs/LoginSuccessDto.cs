using MyApi.Application.DTOs.ExternalServices.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Users.DTOs
{
    public record LoginSuccessDto
    {
        public TokenResponseDto Tokens { get; init; } = null!;
        public UserLoginResponseDto User { get; init; } = null!;
    }
}
