using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Abstraction.Services
{
    public interface IUserRoleClaimService
    {
        Task<UserRolesAndClaimsDto> GetRolesAndClaimsAsync(AppUser user);
    }
}
