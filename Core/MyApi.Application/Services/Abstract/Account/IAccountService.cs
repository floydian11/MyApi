using MyApi.Application.DTOs.Account;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.Abstract.Account
{
    public interface IAccountService
    {
        // 1️⃣ Authentication
        Task<IDataResult<UserResponseDto?>> RegisterUserAsync(RegisterDto dto);
        Task<IDataResult<UserLoginResponseDto?>> LoginAsync(LoginDto dto);
        Task<IResult> LogoutAsync();

        // 2️⃣ User management
        Task<IDataResult<UserResponseDto?>> GetUserByIdAsync(Guid userId);
        Task<IDataResult<IEnumerable<UserResponseDto>>> GetAllUsersAsync();
        Task<IDataResult<UserResponseDto?>> UpdateUserAsync(Guid userId, UpdateUserDto dto);
        Task<IResult> DeleteUserAsync(Guid userId);
        Task<IResult> ActivateUserAsync(Guid userId);
        Task<IResult> DeactivateUserAsync(Guid userId);

        // 3️⃣ Role management
        Task<IDataResult<IEnumerable<string>>> GetRolesAsync();
        Task<IResult> AddRoleAsync(string roleName);
        Task<IResult> DeleteRoleAsync(string roleName);
        Task<IResult> AssignRoleToUserAsync(Guid userId, string roleName);
        Task<IResult> RemoveRoleFromUserAsync(Guid userId, string roleName);
        Task<IDataResult<IEnumerable<string>>> GetUserRolesAsync(Guid userId);

        // 4️⃣ Claim/Permission management
        Task<IResult> AddClaimToUserAsync(Guid userId, ClaimDto claim);
        Task<IResult> RemoveClaimFromUserAsync(Guid userId, ClaimDto claim);
        Task<IDataResult<IEnumerable<ClaimDto>>> GetUserClaimsAsync(Guid userId);

        // 5️⃣ Password/Security
        Task<IResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
        Task<IResult> ResetPasswordAsync(Guid userId, string newPassword);
        Task<bool> VerifyPasswordAsync(Guid userId, string password);
    }
}
