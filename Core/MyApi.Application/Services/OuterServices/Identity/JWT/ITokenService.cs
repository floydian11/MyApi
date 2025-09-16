using MyApi.Application.DTOs.ExternalServices.Account;
using MyApi.Application.Results;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.OuterServices.Identity.JWT
{
    public interface ITokenService
    {
        Task<TokenResponseDto> GenerateTokensAsync(AppUser user, IEnumerable<string> roles, IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        Task<IDataResult<TokenResponseDto>> RefreshAccessTokenAsync(string refreshToken);
       

    }
}
