using MediatR;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Results;
using MyApi.Application.Services.OuterServices.Identity.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<TokenResponseDto>>
    {
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<Result<TokenResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // TokenService'teki metodu çağırıyoruz. O zaten IDataResult dönüyor.
            // Bizim bunu kendi Result desenimize çevirmemiz gerekiyor.
            var result = await _tokenService.RefreshAccessTokenAsync(request.RefreshToken);

            if (!result.Success)
            {
                // TokenService'ten gelen hata mesajını kendi Hata nesnemize saralım.
                return Result.Failure<TokenResponseDto>(AuthenticationErrors.RefreshFailed);
            }

            // TokenService'ten gelen veriyi kendi Success Result'ımıza saralım.
            return Result.Success(result.Data!);
        }
    }
}
