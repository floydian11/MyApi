using MediatR;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.Features.Identity.Roles;
using MyApi.Application.Features.Identity.Roles.Commands.DeleteRole;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories.Claim;
using MyApi.Application.Results;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Commands.DeleteClaimTemplate
{
    public class DeleteClaimTemplateCommandHandler : IRequestHandler<DeleteClaimTemplateCommand, Result>
    {
        private readonly IClaimTemplateRepository _claimTemplateRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public DeleteClaimTemplateCommandHandler(IClaimTemplateRepository claimTemplateRepository, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _claimTemplateRepository = claimTemplateRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result> Handle(DeleteClaimTemplateCommand request, CancellationToken cancellationToken)
        {
            // 1. Adım: Silinmek istenen rolü veritabanından bul.
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            if (role == null)
            {
                // Rol yoksa, standart NotFound hatasını dön.
                return Result.Failure(RoleErrors.NotFound);
            }

            // 2. Adım: Güvenlik Kontrolü - Bu rol herhangi bir kullanıcıya atanmış mı?
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
            if (usersInRole.Any())
            {
                // Eğer atanmışsa, hangi kullanıcılarda olduğunu belirten dinamik hata mesajını dön.
                var userNames = string.Join(", ", usersInRole.Select(u => u.UserName));
                return Result.Failure(RoleErrors.InUseByUser(userNames));
            }

            // 3. Adım: Güvenlik Kontrolü - Bu role herhangi bir claim (yetki) atanmış mı?
            var claimsInRole = await _roleManager.GetClaimsAsync(role);
            if (claimsInRole.Any())
            {
                // Eğer atanmışsa, silmeyi engelle ve hata dön.
                return Result.Failure(RoleErrors.HasClaims);
            }

            // 4. Adım: Tüm kontrollerden başarıyla geçtiyse, rol artık güvenle silinebilir.
            var identityResult = await _roleManager.DeleteAsync(role);

            // 5. Adım: Sonucu, uygun Result nesnesine çevirerek dön.
            return identityResult.Succeeded
                ? Result.Success() //mesaj parantez içine yazılabilir. yoksa frontende default mesaj gösterilir.
                : Result.Failure(RoleErrors.DeletionFailed);
        }

        
    }
}
