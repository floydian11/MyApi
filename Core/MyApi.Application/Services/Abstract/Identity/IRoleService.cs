using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.Abstract.Identity
{
    public interface IRoleService
    {
        Task<IDataResult<IEnumerable<string>>> GetRolesAsync();
        Task<IResult> AddRoleAsync(string roleName);
        Task<IResult> DeleteRoleAsync(string roleName);
        Task<IResult> AssignRoleToUserAsync(Guid userId, string roleName);
        Task<IResult> RemoveRoleFromUserAsync(Guid userId, string roleName);
        Task<IDataResult<IEnumerable<string>>> GetUserRolesAsync(Guid userId);
    }
}
