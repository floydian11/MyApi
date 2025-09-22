using MediatR;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Authentication.Commands.RefreshToken
{
    // Girdi olarak eski RefreshToken'ı alır, çıktı olarak yeni bir TokenResponseDto bekler.
    public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<TokenResponseDto>>;
}
