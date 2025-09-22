using MediatR;
using MyApi.Application.Features.Identity.Claims.DTOs;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Queries.GetClaimTemplateById
{
    public record GetClaimTemplateByIdQuery(Guid Id) : IRequest<Result<ClaimTemplateDto>>;
}
