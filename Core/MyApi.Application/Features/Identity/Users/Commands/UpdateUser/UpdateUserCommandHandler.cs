using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.Abstraction.Services;
using MyApi.Application.DTOs;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Features.Identity.Users.Queries.GetUserById;
using MyApi.Application.Results;
using MyApi.Application.Results.Eski;
using MyApi.Application.Services.OuterServices.Identity.Hash;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Result = MyApi.Application.Results.Result;

namespace MyApi.Application.Features.Identity.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserResponseDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;
        private readonly IUserRoleClaimService _userRoleClaimService;

        public UpdateUserCommandHandler(UserManager<AppUser> userManager, IMapper mapper, IHashService hashService, IUserRoleClaimService userRoleClaimService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _hashService = hashService;
            _userRoleClaimService = userRoleClaimService;
        }

        public async Task<Result<UserResponseDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
                return Result.Failure<UserResponseDto>(UserErrors.NotFound);

            // DTO'dan gelen null olmayan değerleri güncelle.
            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.Email = request.Email ?? user.Email;
            user.UserName = request.Username ?? user.UserName;

            // Eğer TCKN güncelleniyorsa, hash'leyerek kaydet.
            if (!string.IsNullOrEmpty(request.TCKN))
            {
                var (tcknHash, tcknSalt) = _hashService.HashValue(request.TCKN);
                user.TCKNHash = tcknHash;
                user.TCKNSalt = tcknSalt;
            }

            // Adminler için IsActive durumunu güncelleme.
            if (request.IsActive.HasValue)
            {
                user.IsActive = request.IsActive.Value;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return Result.Failure<UserResponseDto>(UserErrors.UpdateFailed);
            }

            // Başarılı güncelleme sonrası güncel kullanıcı bilgilerini döndür.
            // --- DÜZELTİLMİŞ KISIM BURASI ---
            // 4. Başarılı güncelleme sonrası, kullanıcıya güncel ve tam bilgiyi geri dön.

            // a. Güncellenmiş kullanıcının rollerini ve claim'lerini çek.
            // 2. Roller ve Claim'leri veritabanından AYNI ANDA istemek için Task'leri başlat.
            // Bu, önce rolleri bekleyip sonra claim'leri istemekten daha performanslıdır.
            //var rolesTask = _userManager.GetRolesAsync(user);
            //var claimsTask = _userManager.GetClaimsAsync(user);

            //// Her iki asenkron işlemin de bitmesini bekle.
            //await Task.WhenAll(rolesTask, claimsTask);

            //// Tamamlanan task'lerden sonuçları al.
            //var userRoles = await rolesTask;
            //var userClaims = await claimsTask;
            //YUKARIDAKİ YAPIYI MERKEZİLEŞTİRDİK ARTIK AŞAĞIDAKİ SERVİSTEN ÇAĞIRIYORUZ.
            // Tekrar eden kod bloğu yerine, tek satırlık servis çağrısı!
           

            // b. AutoMapper ile temel DTO'yu oluştur.
            var userDto = _mapper.Map<UserResponseDto>(user);

            var rolesAndClaims = await _userRoleClaimService.GetRolesAndClaimsAsync(user);

            // c. 'with' ifadesiyle DTO'yu roller ve claim'lerle zenginleştir.
            userDto = userDto with
            {
                Roles = rolesAndClaims.Roles,
                Claims = _mapper.Map<IEnumerable<ClaimDto>>(rolesAndClaims.Claims)
            };

            // d. Zenginleştirilmiş ve güncel DTO ile başarılı sonucu dön.
            return Result.Success(userDto);
            //return Result.Success(userDto, "Kullanıcı başarıyla kaydedildi."); BURASI RESULT SINFIINDA A'DAKİ MESAJ YAPISINI KULLANIRSA KULLANILACAK. 
            //biz şimdi kullanmıyoruz. başarı mesajını frontendde vereceğiz.
        }
    }
}
//"Eğer request.FirstName'in içinde bir değer varsa (yani null değilse),
//user.FirstName'i bu yeni değere ata. Eğer request.FirstName null ise,
//user.FirstName'in mevcut değerine dokunma, onu olduğu gibi bırak (yani kendisine ata)."