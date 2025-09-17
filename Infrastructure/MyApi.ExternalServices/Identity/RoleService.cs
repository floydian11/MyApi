using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Results;
using MyApi.Application.Results.Eski;
using MyApi.Application.Services.OuterServices.Identity;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.ExternalServices.Identity
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleService(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IDataResult<IEnumerable<string>>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return new SuccessDataResult<IEnumerable<string>>(roles!);
        }

        // Sisteme yeni bir rol ekler.
        public async Task<IResult> AddRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return new ErrorResult("Bu rol zaten mevcut.");

            var result = await _roleManager.CreateAsync(new AppRole { Name = roleName });
            return result.Succeeded ? new SuccessResult("Rol başarıyla oluşturuldu.") : new ErrorResult("Rol oluşturulamadı.");
        }

        public async Task<IResult> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return new ErrorResult("Rol bulunamadı.");

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded ? new SuccessResult("Rol silindi.") : new ErrorResult("Rol silinemedi.");
        }

        // Bir kullanıcıya rol atar.
        public async Task<IResult> AssignRoleToUserAsync(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return new ErrorResult("Kullanıcı bulunamadı.");

            if (!await _roleManager.RoleExistsAsync(roleName))
                return new ErrorResult("Rol bulunamadı.");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded ? new SuccessResult("Rol kullanıcıya atandı.") : new ErrorResult("Rol atanamadı.");
        }

        public async Task<IResult> RemoveRoleFromUserAsync(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return new ErrorResult("Kullanıcı bulunamadı.");

            if (!await _roleManager.RoleExistsAsync(roleName))
                return new ErrorResult("Rol bulunamadı.");

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded ? new SuccessResult("Rol kullanıcıdan kaldırıldı.") : new ErrorResult("Rol kaldırılamadı.");
        }

        public async Task<IDataResult<IEnumerable<string>>> GetUserRolesAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return new ErrorDataResult<IEnumerable<string>>(null, "Kullanıcı bulunamadı.");

            var roles = await _userManager.GetRolesAsync(user);
            return new SuccessDataResult<IEnumerable<string>>(roles);
        }
    }
}
