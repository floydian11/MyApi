using MediatR;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.Features.Identity.Users;
using MyApi.Application.Results;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Commands.RemoveClaimFromUser
{
    public class RemoveClaimFromUserCommandHandler : IRequestHandler<RemoveClaimFromUserCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;

        public RemoveClaimFromUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(RemoveClaimFromUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return Result.Failure(UserErrors.NotFound);

            var claimToRemove = new Claim(request.ClaimType, request.ClaimValue);

            // Kullanıcının bu claim'e sahip olup olmadığını kontrol etmek iyi bir adımdır.
            var userClaims = await _userManager.GetClaimsAsync(user);
            if (!userClaims.Any(c => c.Type == request.ClaimType && c.Value == request.ClaimValue))
            {
                return Result.Failure(ClaimErrors.NotAssigned);
            }

            var identityResult = await _userManager.RemoveClaimAsync(user, claimToRemove);

            return identityResult.Succeeded
                ? Result.Success()
                : Result.Failure(ClaimErrors.RemoveFailed);
        }
    }
}
