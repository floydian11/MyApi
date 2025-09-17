using MyApi.Application.DTOs.ExternalServices.Account;
using MyApi.Application.DTOs.ExternalServices.Identity;
using MyApi.Application.Results.Eski;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.OuterServices.Identity
{
    public interface IClaimService
    {
        Task<IResult> AddClaimToUserAsync(Guid userId, ClaimDto claim);
        Task<IResult> RemoveClaimFromUserAsync(Guid userId, ClaimDto claim);
        Task<IDataResult<IEnumerable<ClaimDto>>> GetUserClaimsAsync(Guid userId);
    }
}
