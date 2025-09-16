using MyApi.Application.DTOs.ExternalServices.Account;
using MyApi.Application.DTOs.ExternalServices.Identity;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.OuterServices.Identity
{
    public interface IUserService
    {
        Task<IDataResult<UserResponseDto?>> GetUserByIdAsync(Guid userId);
        Task<IDataResult<IEnumerable<UserResponseDto>>> GetAllUsersAsync();
        Task<IDataResult<IEnumerable<UserListDto>>> GetAllUsersForListAsync();
        Task<IDataResult<UserResponseDto?>> UpdateUserAsync(Guid userId, UpdateUserDto dto);
        Task<IResult> DeleteUserAsync(Guid userId);
        Task<IResult> ActivateUserAsync(Guid userId);
        Task<IResult> DeactivateUserAsync(Guid userId);
    }
}
