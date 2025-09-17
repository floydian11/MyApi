using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.DTOs.ExternalServices.Account;
using MyApi.Application.DTOs.ExternalServices.Identity;
using MyApi.Application.Results.Eski;
using MyApi.Application.Services.OuterServices.Identity;
using MyApi.Application.Services.OuterServices.Identity.Hash;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.ExternalServices.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;

        public UserService(UserManager<AppUser> userManager, IMapper mapper, IHashService hashService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _hashService = hashService;
        }

        // ID ile tek bir kullanıcı getirir.
        public async Task<IDataResult<UserResponseDto?>> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new ErrorDataResult<UserResponseDto?>(null, "Kullanıcı bulunamadı.");

            // Kullanıcıyı DTO'ya map'le.
            var userDto = _mapper.Map<UserResponseDto>(user);

            // DTO için rolleri ve claim'leri manuel olarak alıp ekliyoruz.
            userDto.Roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);
            userDto.Claims = _mapper.Map<IEnumerable<ClaimDto>>(claims);

            return new SuccessDataResult<UserResponseDto?>(userDto);
        }

        
        // YENİ VE HIZLI LİSTELEME METODU
        // Frontend'deki kullanıcı listesi ekranı için bunu kullanacağız.
        public async Task<IDataResult<IEnumerable<UserListDto>>> GetAllUsersForListAsync()
        {
            // Sadece gereken alanları veritabanından çeker. Roller/Claim'ler yok.
            // Bu işlem tek bir veritabanı sorgusu ile yapılır.
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

            return new SuccessDataResult<IEnumerable<UserListDto>>(users);
        }

        // Bir kullanıcının bilgilerini günceller.
        public async Task<IDataResult<UserResponseDto?>> UpdateUserAsync(Guid userId, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new ErrorDataResult<UserResponseDto?>(null, "Güncellenecek kullanıcı bulunamadı.");

            // DTO'dan gelen null olmayan değerleri güncelle.
            user.FirstName = dto.FirstName ?? user.FirstName;
            user.LastName = dto.LastName ?? user.LastName;
            user.Email = dto.Email ?? user.Email;
            user.UserName = dto.Username ?? user.UserName;

            // Eğer TCKN güncelleniyorsa, hash'leyerek kaydet.
            if (!string.IsNullOrEmpty(dto.TCKN))
            {
                var (tcknHash, tcknSalt) = _hashService.HashValue(dto.TCKN);
                user.TCKNHash = tcknHash;
                user.TCKNSalt = tcknSalt;
            }

            // Adminler için IsActive durumunu güncelleme.
            if (dto.IsActive.HasValue)
            {
                user.IsActive = dto.IsActive.Value;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ErrorDataResult<UserResponseDto?>(null, $"Güncelleme başarısız: {errors}");
            }

            // Başarılı güncelleme sonrası güncel kullanıcı bilgilerini döndür.
            return await GetUserByIdAsync(userId);
        }

        // Bir kullanıcıyı siler.
        public async Task<IResult> DeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new ErrorResult("Silinecek kullanıcı bulunamadı.");

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded
                ? new SuccessResult("Kullanıcı başarıyla silindi.")
                : new ErrorResult("Kullanıcı silinemedi.");
        }

        // Bir kullanıcıyı aktif eder.
        public async Task<IResult> ActivateUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return new ErrorResult("Kullanıcı bulunamadı.");

            user.IsActive = true;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded
                ? new SuccessResult("Kullanıcı aktif edildi.")
                : new ErrorResult("İşlem başarısız.");
        }

        // Bir kullanıcıyı pasif hale getirir.
        public async Task<IResult> DeactivateUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return new ErrorResult("Kullanıcı bulunamadı.");

            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded
                ? new SuccessResult("Kullanıcı pasif hale getirildi.")
                : new ErrorResult("İşlem başarısız.");
        }

        // ESKİ GetAllUsersAsync METODUNUN OPTİMİZE EDİLMİŞ HALİ
        // Bu metod, tüm kullanıcıların tüm detaylarını (roller, claim'ler) tek seferde ve performanslı bir şekilde çeker.
        // Nadiren, belki bir raporlama ekranı için kullanılabilir.
        public async Task<IDataResult<IEnumerable<UserResponseDto>>> GetAllUsersAsync() // İsmini daha açıklayıcı yaptım
        {
            var usersWithDetails = await _userManager.Users
                .Include(u => u.UserRoles) // İlişkili UserRoles tablosunu çek
                .ThenInclude(ur => ur.Role) // UserRoles üzerinden Role bilgilerini çek
                .Include(u => u.UserClaims) // İlişkili UserClaims tablosunu çek
                .Select(u => new UserResponseDto
                {
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

            return new SuccessDataResult<IEnumerable<UserResponseDto>>(usersWithDetails);
        }

        // Tüm kullanıcıları listeler. ÜSTE OPTIMIZE EDİLMİŞ VERSİYONUNU KULLANACAĞIZ.n+1 problemine yol açabilir.
        //public async Task<IDataResult<IEnumerable<UserResponseDto>>> GetAllUsersAsync()
        //{
        //    var users = await _userManager.Users.ToListAsync();
        //    var userDtos = new List<UserResponseDto>();

        //    // Not: Çok fazla kullanıcı varsa (1000+), bu döngü N+1 sorgu problemine yol açabilir.
        //    // Performans kritikse, roller ve claim'ler için daha optimize bir sorgu gerekebilir.
        //    foreach (var user in users)
        //    {
        //        var userDto = _mapper.Map<UserResponseDto>(user);
        //        userDto.Roles = await _userManager.GetRolesAsync(user);
        //        var claims = await _userManager.GetClaimsAsync(user);
        //        userDto.Claims = _mapper.Map<IEnumerable<ClaimDto>>(claims);
        //        userDtos.Add(userDto);
        //    }

        //    return new SuccessDataResult<IEnumerable<UserResponseDto>>(userDtos);
        //}

    }
}
