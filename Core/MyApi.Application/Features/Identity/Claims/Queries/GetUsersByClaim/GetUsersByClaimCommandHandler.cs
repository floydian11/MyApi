using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Results;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Queries.GetUsersByClaim
{
    public class GetUsersByClaimQueryHandler : IRequestHandler<GetUsersByClaimQuery, Result<List<UserResponseDto>>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public GetUsersByClaimQueryHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result<List<UserResponseDto>>> Handle(GetUsersByClaimQuery request, CancellationToken cancellationToken)
        {
            var claim = new Claim(request.ClaimType, request.ClaimValue);

            // UserManager'ın bu iş için hazır harika bir metodu var.
            var usersWithClaim = await _userManager.GetUsersForClaimAsync(claim);

            // Bulunan AppUser listesini UserResponseDto listesine çevir.
            // Not: Bu, her kullanıcı için rol/claim'leri tekrar çekebilir. 
            // Daha performanslı bir çözüm için bu map'lemeyi manuel yapmak gerekebilir.
            // Ama şimdilik AutoMapper ile devam edelim.
            var userDtos = _mapper.Map<List<UserResponseDto>>(usersWithClaim);

            return Result.Success(userDtos);
        }
    }
}
