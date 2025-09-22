using MediatR;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.Features.Identity.Users;
using MyApi.Application.Results;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles.Commands.DeleteRole
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result>
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public DeleteRoleCommandHandler(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            if (role == null)
                return Result.Failure(RoleErrors.NotFound);

            // 1. ÖN KONTROL: Bu role atanmış herhangi bir kullanıcı var mı?
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
            if (usersInRole.Any())
            {
                // Eğer varsa, hatayla işlemi durdur.
                return Result.Failure(RoleErrors.RoleInUse);
            }

            // 2. ÖN KONTROL (İsteğe Bağlı ama İyi Pratik): Bu role atanmış claim var mı?
            var claimsInRole = await _roleManager.GetClaimsAsync(role);
            if (claimsInRole.Any())
            {
                return Result.Failure(RoleErrors.HasClaims);
            }

            // Tüm kontrollerden geçtiyse, rol artık güvenle silinebilir.
            var identityResult = await _roleManager.DeleteAsync(role);

            return identityResult.Succeeded
                ? Result.Success() : Result.Failure(RoleErrors.DeletionFailed);
        }
    }
    
}
