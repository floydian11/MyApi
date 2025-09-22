using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Features.Identity.Users.Queries.GetAllUsers;
using MyApi.Application.Results;
using MyApi.Application.Results.Eski;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Result = MyApi.Application.Results.Result;

namespace MyApi.Application.Features.Identity.Users.Queries.GetAllUsersForList
{
    public class GetAllUsersForListQueryHandler : IRequestHandler<GetAllUsersForListQuery, Result<List<UserListDto>>>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetAllUsersForListQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<List<UserListDto>>> Handle(GetAllUsersForListQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
               .Select(u => new UserListDto
               {
                   Id = u.Id,
                   Username = u.UserName!,
                   Email = u.Email!,
                   FullName = u.FirstName + " " + u.LastName,
                   IsActive = u.IsActive
               })
               .ToListAsync();

            return Result.Success(users);
        }
    }
}
