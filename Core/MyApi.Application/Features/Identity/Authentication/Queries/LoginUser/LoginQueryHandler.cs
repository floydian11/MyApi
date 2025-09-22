using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.Features.Identity.Users;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Results;
using MyApi.Application.Services.OuterServices.Identity.JWT;
using MyApi.Domain.Entities.Identity;

namespace MyApi.Application.Features.Identity.Authentication.Queries.LoginUser
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<LoginSuccessDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public LoginQueryHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<Result<LoginSuccessDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            // Kullanıcıyı kullanıcı adına göre bul.
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                return Result.Failure<LoginSuccessDto>(UserErrors.EmailInUse);

            // Kullanıcının hesabı aktif mi? (Eğer AppUser'da IsActive alanı varsa)
            if (!user.IsActive)
                return Result.Failure<LoginSuccessDto>(UserErrors.InvalidCredentials);

            if (!user.IsActive)
                // DÜZELTME: Pasif hesaplar için ayrı bir hata kodu daha anlamlı olur.
                return Result.Failure<LoginSuccessDto>(UserErrors.AccountNotActive);


            // SignInManager, parola kontrolü, lockout gibi güvenlik mekanizmalarını yönetir.
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
                return Result.Failure<LoginSuccessDto>(UserErrors.LoginFailed);

            // Parola doğruysa, kullanıcının rollerini ve claim'lerini al.
            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            // TokenService aracılığıyla yeni token'lar üret.
            var tokens = await _tokenService.GenerateTokensAsync(user, roles, claims);
            
            var userResponse = _mapper.Map<UserLoginResponseDto>(user);

            userResponse = userResponse with
            {
                Roles = roles,
                Claims = _mapper.Map<List<ClaimDto>>(claims)
            };

            // Sonucu UserLoginResponseDto olarak hazırla.
            //var userResponse = new UserLoginResponseDto BUNUN YERİNE AUTOMAPPER
            //{
            //    Username = user.UserName!,
            //    Email = user.Email!,
            //    Roles = roles,
            //    Claims = claims.Select(c => new ClaimDto { Type = c.Type, Value = c.Value }).ToList(),
            //    // Token bilgilerini de cevaba ekleyebiliriz. Bu, frontend'in ihtiyacına bağlı.
            //    // Genellikle token'lar ayrı bir nesne içinde döner, biz de öyle yapalım.
            //    // Bu yüzden UserLoginResponseDto'ya bir TokenResponseDto propertysi eklemek iyi olabilir.
            //    // Şimdilik ayrı tutuyorum. Frontend hem kullanıcı bilgisini hem de token'ı alacak.
            //};

            // Yeni LoginSuccessDto'yu oluşturup hem token'ları hem de kullanıcı bilgilerini içine koyuyoruz.
            //BURAYI 13.soruda gelen cevaba ve aşağıdaki notlara göre düzenledim. loginsuccessdto oluşturdum. burada iki dto yu birleştiriyorum.
            var loginSuccessResponse = new LoginSuccessDto
            {
                Tokens = tokens,
                User = userResponse
            };

            return Result.Success(loginSuccessResponse);
            //Result.Success<LoginSuccessDto>(loginSuccessResponse); İKİ ŞEKİLDE DE OLUYOR. BİRİSİ GENERİC DİĞERİ DEĞİL. İKİSİNİN DE DÖNDÜRDÜĞÜ TÜR AYNI.

            // Login işleminin asıl çıktısı token'dır. Başarılı mesajı ile birlikte token'ları döndürebiliriz.
            // Ama interface'imiz UserLoginResponseDto bekliyor. Bu durumda, iki bilgiyi de içeren bir DTO daha mantıklı olabilir.
            // Mevcut DTO'ları bozmadan devam edelim, controller seviyesinde iki bilgiyi birleştirebiliriz.
            // Örnek: return new SuccessDataResult<object>(new { User = response, Token = tokens });
            // Şimdilik interface'e sadık kalalım. Token'ları RefreshTokenAsync'den alacaklarını varsayalım.
            // En iyi pratik: Login sonucu token'ları ve temel kullanıcı bilgilerini döndürmektir.
            // UserLoginResponseDto'ya TokenResponseDto'yu da ekleyelim.
            // public TokenResponseDto Tokens { get; set; } gibi. Şimdilik mevcut yapıyla devam ediyorum.
            // Controller'da iki ayrı çağrı yapılması gerekecek bu durumda. Bu verimsiz olur.
            // Login işleminin sonucunu değiştirelim ve hem token hem kullanıcı bilgisi döndürelim.
            // Yeni bir DTO oluşturmak en temizi:
            // public class LoginSuccessDto { UserLoginResponseDto User; TokenResponseDto Tokens; }
            // Şimdilik interface'e sadık kalalım ve sadece UserLoginResponseDto döndürelim. Token'lar için ayrı bir endpoint çağrısı gerektiğini varsayalım.
            // Düzeltme: En iyi yaklaşım, login'in token döndürmesidir. Interface'i buna göre düzenlemek daha mantıklı.
            // IAuthService -> Task<IDataResult<TokenResponseDto>> LoginAsync(LoginDto dto); olmalı.
            // Mevcut interface'i koruyarak devam ediyorum.
            //return new SuccessDataResult<LoginSuccessDto?>(loginSuccessResponse, "Giriş başarılı.");
        }
    }
}
