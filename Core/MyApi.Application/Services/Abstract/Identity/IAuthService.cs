using MyApi.Application.DTOs.Account;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.Abstract.Identity
{
    public interface IAuthService
    {
        Task<IDataResult<UserResponseDto?>> RegisterAsync(RegisterDto dto);
        Task<IDataResult<UserLoginResponseDto?>> LoginAsync(LoginDto dto);
        Task<IResult> LogoutAsync();
        Task<IDataResult<TokenResponseDto>> RefreshTokenAsync(string refreshToken);
    }
}
