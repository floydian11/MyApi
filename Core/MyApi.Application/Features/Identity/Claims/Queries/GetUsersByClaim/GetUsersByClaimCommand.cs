using MediatR;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Queries.GetUsersByClaim
{
    // Sonuç olarak UserResponseDto listesi dönecek.
    public record GetUsersByClaimQuery(string ClaimType, string ClaimValue)
        : IRequest<Result<List<UserResponseDto>>>;
}
