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

namespace MyApi.Application.Features.Identity.Roles.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AssignRoleToUserCommandHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            // 1. Kullanıcı var mı diye kontrol et.
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Result.Failure(UserErrors.NotFound); // Kullanıcı bulunamadı hatası
            }

            // 2. Atanacak rol sistemde var mı diye kontrol et.
            var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
            if (!roleExists)
            {
                return Result.Failure(RoleErrors.NotFound); // Rol bulunamadı hatası
            }

            // 3. Kullanıcı zaten bu rolde mi diye kontrol et.
            var userIsInRole = await _userManager.IsInRoleAsync(user, request.RoleName);
            if (userIsInRole)
            {
                return Result.Failure(RoleErrors.UserAlreadyInRole); // Çakışma hatası
            }

            // 4. Tüm kontrollerden geçtiyse, rolü ata.
            var identityResult = await _userManager.AddToRoleAsync(user, request.RoleName);

            // 5. Identity'den dönen sonucu kontrol et ve uygun Result'ı dön.
            return identityResult.Succeeded
                ? Result.Success("Rol kullanıcıya başarıyla atandı.") // İsteğe bağlı başarı mesajı. boş da bırakılabilir.
                : Result.Failure(RoleErrors.AssignFailed);
        }    
    }
}
