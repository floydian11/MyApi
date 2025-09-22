using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Results;
using MyApi.Application.Results.Eski;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Result = MyApi.Application.Results.Result;

namespace MyApi.Application.Features.Identity.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserResponseDto>>>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetAllUsersQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<List<UserResponseDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var usersWithDetails = await _userManager.Users
               .Include(u => u.UserRoles) // İlişkili UserRoles tablosunu çek
               .ThenInclude(ur => ur.Role) // UserRoles üzerinden Role bilgilerini çek
               .Include(u => u.UserClaims) // İlişkili UserClaims tablosunu çek
               .Select(u => new UserResponseDto
               {
                   Id = u.Id,
                   Username = u.UserName!,
                   Email = u.Email!,
                   FirstName = u.FirstName,
                   LastName = u.LastName,
                   // Önceden yüklediğimiz (Include) veriyi kullanarak rolleri ve claim'leri alıyoruz.
                   // Artık her kullanıcı için ayrı sorgu ATMIYORUZ.
                   Roles = u.UserRoles.Select(ur => ur.Role.Name!),
                   Claims = u.UserClaims.Select(uc => new ClaimDto
                   {
                       Type = uc.ClaimType ?? string.Empty,
                       Value = uc.ClaimValue ?? string.Empty
                   })
               })
               .ToListAsync();

            return Result.Success(usersWithDetails);
        }
    }
}
