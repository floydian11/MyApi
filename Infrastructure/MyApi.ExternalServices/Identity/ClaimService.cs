using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.DTOs.ExternalServices.Identity;
using MyApi.Application.Results;
using MyApi.Application.Services.OuterServices.Identity;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.ExternalServices.Identity
{
    public class ClaimService : IClaimService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ClaimService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IResult> AddClaimToUserAsync(Guid userId, ClaimDto claimDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return new ErrorResult("Kullanıcı bulunamadı.");

            var claim = new Claim(claimDto.Type, claimDto.Value);
            var result = await _userManager.AddClaimAsync(user, claim);

            return result.Succeeded ? new SuccessResult("Claim kullanıcıya eklendi.") : new ErrorResult("Claim eklenemedi.");
        }

        public async Task<IResult> RemoveClaimFromUserAsync(Guid userId, ClaimDto claimDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return new ErrorResult("Kullanıcı bulunamadı.");

            var claim = new Claim(claimDto.Type, claimDto.Value);
            var result = await _userManager.RemoveClaimAsync(user, claim);

            return result.Succeeded ? new SuccessResult("Claim kullanıcıdan kaldırıldı.") : new ErrorResult("Claim kaldırılamadı.");
        }

        public async Task<IDataResult<IEnumerable<ClaimDto>>> GetUserClaimsAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return new ErrorDataResult<IEnumerable<ClaimDto>>(null, "Kullanıcı bulunamadı.");

            var claims = await _userManager.GetClaimsAsync(user);
            var claimDtos = _mapper.Map<IEnumerable<ClaimDto>>(claims);

            return new SuccessDataResult<IEnumerable<ClaimDto>>(claimDtos);
        }
    }
}
