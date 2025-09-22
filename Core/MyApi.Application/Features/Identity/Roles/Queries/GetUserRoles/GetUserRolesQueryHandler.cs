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

namespace MyApi.Application.Features.Identity.Roles.Queries.GetUserRoles
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, Result<IList<string>>>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetUserRolesQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<IList<string>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            // 1. İlgili kullanıcıyı ID ile bul.
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                // Kullanıcı yoksa NotFound hatası dön.
                return Result.Failure<IList<string>>(UserErrors.NotFound);
            }

            // 2. UserManager üzerinden kullanıcının rollerini çek.
            var roles = await _userManager.GetRolesAsync(user);

            // 3. Başarılı sonucu ve rol listesini dön.
            return Result.Success(roles);
        }
    }
}
