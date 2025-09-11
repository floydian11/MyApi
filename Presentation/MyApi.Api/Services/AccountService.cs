using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.DTOs.Account;
using MyApi.Application.Results;
using MyApi.Application.Services.Abstract;
using MyApi.Application.Services.Abstract.Account;
using MyApi.Application.Utilities;
using MyApi.Domain.Entities.Identity;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyApi.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;

        public AccountService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IMapper mapper,
            IHashService hashService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _hashService = hashService;
        }


        public async Task<IDataResult<UserLoginResponseDto?>> LoginAsync(LoginDto dto)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                // 1. Kullanıcı adı ile bul
                var user = await _userManager.FindByNameAsync(dto.Username);
                if (user == null)
                    throw new Exception("Kullanıcı bulunamadı");

                // 2. Şifre kontrolü
                var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
                if (!result.Succeeded)
                    throw new Exception("Kullanıcı adı veya şifre hatalı");

                // 3. DTO’ya map et
                var response = _mapper.Map<UserLoginResponseDto>(user);
                response.Roles = await _userManager.GetRolesAsync(user);
                response.Claims = (await _userManager.GetClaimsAsync(user))
                                  .Select(c => new ClaimDto { Type = c.Type, Value = c.Value });

                return response;

            }, successMessage: "Login başarılı",
               errorMessage: "Login başarısız");
        }


        public async Task<IDataResult<UserResponseDto?>> RegisterUserAsync(RegisterDto dto)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                var userExists = await _userManager.FindByEmailAsync(dto.Email);
                if (userExists != null)
                    throw new Exception("Email zaten kayıtlı.");

                var (hash, salt) = _hashService.HashValue(dto.TCKN);
                 
                //var user = new AppUser AŞAĞIDAKİ DAHA TEMİZ
                //{
                //    UserName = dto.Username,
                //    Email = dto.Email,
                //    FirstName = dto.FirstName,
                //    LastName = dto.LastName,
                //    TCKNHash = hash,
                //    TCKNSalt = salt,
                //    CreatedDate = DateTime.UtcNow,
                //    IsActive = true
                //};

                var user = _mapper.Map<AppUser>(dto);
                user.TCKNHash = hash;
                user.TCKNSalt = salt;
                user.CreatedDate = DateTime.UtcNow;
                user.IsActive = true;

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

                return _mapper.Map<UserResponseDto>(user);
            },
            successMessage: "Kullanıcı başarıyla oluşturuldu.",
            errorMessage: "Kullanıcı oluşturulamadı.");
        }

    }
}
