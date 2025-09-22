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
            // 1. Önce rolleri al ve bu işlemin 'await' ile TAMAMEN BİTMESİNİ BEKLE.
            var roles = await _userManager.GetRolesAsync(user);

            // 2. Roller işlemi bittikten sonra, GÜVENLE claim'leri al
            //    ve bu işlemin de 'await' ile bitmesini bekle.
            var claims = await _userManager.GetClaimsAsync(user);

            // 3. Artık her iki veri de elimizde olduğuna göre, DTO'yu oluşturup dön.
            return new UserRolesAndClaimsDto
            {
                Roles = roles,
                Claims = claims
            };
        }
    }
}
