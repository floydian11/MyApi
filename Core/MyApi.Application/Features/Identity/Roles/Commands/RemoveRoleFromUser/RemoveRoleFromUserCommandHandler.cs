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

namespace MyApi.Application.Features.Identity.Roles.Commands.RemoveRoleFromUser
{
    public class RemoveRoleFromUserCommandHandler : IRequestHandler<RemoveRoleFromUserCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;

        // Bu handler'da RoleManager'a ihtiyacımız yok, çünkü bir rolün varlığını
        // IsInRoleAsync ile dolaylı olarak kontrol edebiliriz.
        // Ama istersen ekleyip RoleExistsAsync kontrolü de yapabilirsin.
        public RemoveRoleFromUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
        {
            // 1. Kullanıcı var mı diye kontrol et.
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Result.Failure(UserErrors.NotFound);
            }

            // 2. Kullanıcının bu rolde olup olmadığını kontrol et.
            // Zaten rolde olmayan bir kullanıcıdan rol kaldırmaya çalışmak bir hatadır.
            var userIsInRole = await _userManager.IsInRoleAsync(user, request.RoleName);
            if (!userIsInRole)
            {
                // Yeni bir hata tanımlayabiliriz.
                return Result.Failure(RoleErrors.UserNotInRole);
            }

            // 3. Tüm kontrollerden geçtiyse, rolü kaldır.
            var identityResult = await _userManager.RemoveFromRoleAsync(user, request.RoleName);

            // 4. Identity'den dönen sonucu kontrol et ve uygun Result'ı dön.
            // RoleErrors'a RemovalFailed gibi yeni bir hata ekleyebiliriz.
            return identityResult.Succeeded
                ? Result.Success()
                : Result.Failure(RoleErrors.RemovalFailed);
        }
    }
}
