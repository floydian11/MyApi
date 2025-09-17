using Microsoft.AspNetCore.Identity;
using MyApi.Application.Abstraction.Services;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Concrete.Services
{

    //ileride services yapısı kalkacak. ama şimdilik kullanıyoruz. ,
    //o yüzden bazı durumlarda özel servis gerekirse Abstraction ve Concrete yapısını kullanıyoruz.
    public class UserRoleClaimService : IUserRoleClaimService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRoleClaimService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserRolesAndClaimsDto> GetRolesAndClaimsAsync(AppUser user)
        {
            var rolesTask = _userManager.GetRolesAsync(user);
            var claimsTask = _userManager.GetClaimsAsync(user);

            await Task.WhenAll(rolesTask, claimsTask);

            return new UserRolesAndClaimsDto
            {
                Roles = await rolesTask,
                Claims = await claimsTask
            };
        }
    }
}
