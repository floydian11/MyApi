using MediatR;
using MyApi.Application.Features.Identity.Claims.DTOs;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Queries.GetUserClaims
{
    // Sonuç olarak ClaimDto listesi dönecek.
    public record GetUserClaimsQuery(Guid UserId) : IRequest<Result<List<ClaimDto>>>;
}
