using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.Abstraction.Services;
using MyApi.Application.DTOs.ExternalServices.Identity;
using MyApi.Application.Features.Identity.Users.Commands.RegisterUser;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Results;
using MyApi.Application.Results.Eski;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClaimDto = MyApi.Application.Features.Identity.Users.DTOs.ClaimDto;
using Result = MyApi.Application.Results.Result;

namespace MyApi.Application.Features.Identity.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserResponseDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserRoleClaimService _userRoleClaimService;

        public GetUserByIdQueryHandler(UserManager<AppUser> userManager, IMapper mapper, IUserRoleClaimService userRoleClaimService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _userRoleClaimService = userRoleClaimService;
        }

        public async Task<Result<UserResponseDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
                return Result.Failure<UserResponseDto>(UserErrors.NotFound);

            // 2. Roller ve Claim'leri veritabanından AYNI ANDA istemek için Task'leri başlat.
            // Bu, önce rolleri bekleyip sonra claim'leri istemekten daha performanslıdır.

            ////BURADAKİ CLAIM VE ROLE GETİRME İŞLEMLERİ İÇİN SERVİS YAZDI. UPDATE'DE ONUN İÇİNDEKİ KODU KULLANIYORUZ.Bruayı bilinçli bıraktım iki kullanımı görmek için
            //var rolesTask = _userManager.GetRolesAsync(user);
            //var claimsTask = _userManager.GetClaimsAsync(user);

            //// Her iki asenkron işlemin de bitmesini bekle.
            //await Task.WhenAll(rolesTask, claimsTask);

            //// Tamamlanan task'lerden sonuçları al.
            //var userRoles = await rolesTask;
            //var userClaims = await claimsTask;
            var rolesAndClaims = await _userRoleClaimService.GetRolesAndClaimsAsync(user);
            // Kullanıcıyı DTO'ya map'le.
            var userDto = _mapper.Map<UserResponseDto>(user);

            // DTO için rolleri ve claim'leri manuel olarak alıp ekliyoruz.

            // 4. 'with' ifadesini, veritabanından çektiğimiz GERÇEK verilerle DTO'yu güncellemek için kullan.
            // Hem rolleri hem de (map'lenmiş) claim'leri yeni kopyaya ekliyoruz.
            userDto = userDto with
            {
                Roles = rolesAndClaims.Roles,
                Claims = _mapper.Map<IEnumerable<ClaimDto>>(rolesAndClaims.Claims) // System.Security.Claims.Claim'i bizim ClaimDto'muza çevir.
            };


            return Result.Success(userDto);
        }
    }
}
