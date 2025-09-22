using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.Features.Identity.Claims.DTOs;
using MyApi.Application.Features.Identity.Users;
using MyApi.Application.Results;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Queries.GetUserClaims
{
    public class GetUserClaimsQueryHandler : IRequestHandler<GetUserClaimsQuery, Result<List<ClaimDto>>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public GetUserClaimsQueryHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result<List<ClaimDto>>> Handle(GetUserClaimsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return Result.Failure<List<ClaimDto>>(UserErrors.NotFound);

            var claims = await _userManager.GetClaimsAsync(user);

            // System.Security.Claims.Claim listesini bizim kendi ClaimDto listemize çevir.
            var claimDtos = _mapper.Map<List<ClaimDto>>(claims);

            return Result.Success(claimDtos);
        }
    }
}
