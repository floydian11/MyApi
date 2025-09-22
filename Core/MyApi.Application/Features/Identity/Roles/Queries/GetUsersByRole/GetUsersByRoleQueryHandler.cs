using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.DTOs.ExternalServices.Identity;
using MyApi.Application.Features.Identity.Users;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Results;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserListDto = MyApi.Application.Features.Identity.Users.DTOs.UserListDto;

namespace MyApi.Application.Features.Identity.Roles.Queries.GetUsersByRole
{
    public class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQuery, Result<IList<UserListDto>>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;

        public GetUsersByRoleQueryHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<Result<IList<UserListDto>>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
        {
            // 1. İlgili kullanıcıyı ID ile bul.
            var role = await _roleManager.FindByNameAsync(request.Name);
            if (role == null)
            {
                // Kullanıcı yoksa NotFound hatası dön.
                return Result.Failure<IList<UserListDto>>(RoleErrors.NotFound);
            }                        

            var usersInRole = await _userManager.GetUsersInRoleAsync(request.Name);

            var userDtos = _mapper.Map<IList<UserListDto>>(usersInRole);

            // 3. Başarılı sonucu ve rol listesini dön.
            return Result.Success<IList<UserListDto>>(userDtos);
        }
    }
}
