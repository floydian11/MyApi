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

namespace MyApi.Application.Features.Identity.Claims.Commands.AddClaimToUser
{
    public class AddClaimToUserCommandHandler : IRequestHandler<AddClaimToUserCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;

        public AddClaimToUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(AddClaimToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return Result.Failure(UserErrors.NotFound);

            // İş Kuralı: Kullanıcının bu claim'e zaten sahip olup olmadığını kontrol et.
            var userClaims = await _userManager.GetClaimsAsync(user);
            if (userClaims.Any(c => c.Type == request.ClaimType && c.Value == request.ClaimValue))
            {
                // Yeni bir hata tanımlayabiliriz.
                return Result.Failure(ClaimErrors.UserAlreadyInClaim);
            }

            var newClaim = new Claim(request.ClaimType, request.ClaimValue);
            var identityResult = await _userManager.AddClaimAsync(user, newClaim);

            return identityResult.Succeeded
                ? Result.Success("Yetki kullanıcıya başarıyla eklendi.")
                : Result.Failure(ClaimErrors.AssignFailed);
        }
    }
}
