using MyApi.Application.DTOs.ExternalServices.Account;
using MyApi.Application.DTOs.ExternalServices.Identity;
using MyApi.Application.Results.Eski;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.OuterServices.Identity
{
    public interface IAuthService
    {
        Task<IDataResult<UserResponseDto?>> RegisterAsync(RegisterDto dto);
        Task<IDataResult<LoginSuccessDto?>> LoginAsync(LoginDto dto);
        Task<IResult> LogoutAsync();
        Task<IDataResult<TokenResponseDto>> RefreshTokenAsync(string refreshToken);
    }
}
