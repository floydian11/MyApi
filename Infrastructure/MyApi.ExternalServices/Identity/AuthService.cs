using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.DTOs.ExternalServices.Account;
using MyApi.Application.DTOs.ExternalServices.Identity;
using MyApi.Application.Results;
using MyApi.Application.Results.Eski;
using MyApi.Application.Services.OuterServices.Identity;
using MyApi.Application.Services.OuterServices.Identity.Hash;
using MyApi.Application.Services.OuterServices.Identity.JWT;
using MyApi.Domain.Entities.Identity;

namespace MyApi.ExternalServices.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMapper mapper,
            IHashService hashService,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _hashService = hashService;
            _tokenService = tokenService;
        }

        public Task<IDataResult<LoginSuccessDto?>> LoginAsync(LoginDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<TokenResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<UserResponseDto?>> RegisterAsync(RegisterDto dto)
        {
            throw new NotImplementedException();
        }

        // Yeni kullanıcı kaydı yapar.
        //public async Task<IDataResult<UserResponseDto?>> RegisterAsync(RegisterDto dto)
        //{
        //    // Kullanıcı adı veya email daha önce alınmış mı kontrol et.
        //    var existingUser = await _userManager.FindByNameAsync(dto.Username);
        //    if (existingUser != null)
        //        return new ErrorDataResult<UserResponseDto?>(null, "Bu kullanıcı adı zaten alınmış.");

        //    var existingEmail = await _userManager.FindByEmailAsync(dto.Email);
        //    if (existingEmail != null)
        //        return new ErrorDataResult<UserResponseDto?>(null, "Bu e-posta adresi zaten kullanılıyor.");

        //    // AutoMapper kullanarak RegisterDto'yu AppUser entity'sine dönüştür.
        //    var newUser = _mapper.Map<AppUser>(dto);

        //    // TCKN'i düz metin olarak DTO'dan alıp hash ve salt'ını oluşturuyoruz.
        //    var (tcknHash, tcknSalt) = _hashService.HashValue(dto.TCKN);
        //    newUser.TCKNHash = tcknHash;
        //    newUser.TCKNSalt = tcknSalt;

        //    // Identity'nin CreateAsync metodu, kullanıcıyı veritabanına ekler ve parolayı hash'leyerek kaydeder.
        //    var result = await _userManager.CreateAsync(newUser, dto.Password);

        //    if (!result.Succeeded)
        //    {
        //        // Eğer Identity bir hata dönerse (örn: parola politikası uyuşmazlığı), hataları birleştirip döndür.
        //        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        //        return new ErrorDataResult<UserResponseDto?>(null, $"Kayıt başarısız: {errors}");
        //    }

        //    // Yeni kullanıcıya varsayılan olarak "User" rolünü ata.
        //    await _userManager.AddToRoleAsync(newUser, "User");

        //    // Başarılı kayıt sonrası, kullanıcı bilgilerini UserResponseDto olarak döndür.
        //    var userResponse = _mapper.Map<UserResponseDto>(newUser);
        //    userResponse.Roles = new List<string> { "User" }; // Rolleri manuel ekliyoruz.

        //    return new SuccessDataResult<UserResponseDto?>(userResponse, "Kullanıcı başarıyla kaydedildi.");
        //}

        //// Kullanıcı girişi yapar ve token'ları döndürür.
        //public async Task<IDataResult<LoginSuccessDto?>> LoginAsync(LoginDto dto)
        //{
        //    // Kullanıcıyı kullanıcı adına göre bul.
        //    var user = await _userManager.FindByNameAsync(dto.Username);
        //    if (user == null)
        //        return new ErrorDataResult<LoginSuccessDto?>(null, "Kullanıcı adı veya şifre hatalı.");

        //    // Kullanıcının hesabı aktif mi? (Eğer AppUser'da IsActive alanı varsa)
        //    if (!user.IsActive)
        //        return new ErrorDataResult<LoginSuccessDto?>(null, "Hesabınız aktif değil. Lütfen yönetici ile iletişime geçin.");

        //    // SignInManager, parola kontrolü, lockout gibi güvenlik mekanizmalarını yönetir.
        //    var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);

        //    if (!result.Succeeded)
        //        return new ErrorDataResult<LoginSuccessDto?>(null, "Kullanıcı adı veya şifre hatalı.");

        //    // Parola doğruysa, kullanıcının rollerini ve claim'lerini al.
        //    var roles = await _userManager.GetRolesAsync(user);
        //    var claims = await _userManager.GetClaimsAsync(user);

        //    // TokenService aracılığıyla yeni token'lar üret.
        //    var tokens = await _tokenService.GenerateTokensAsync(user, roles, claims);

        //    // Sonucu UserLoginResponseDto olarak hazırla.
        //    var userResponse = new UserLoginResponseDto
        //    {
        //        Username = user.UserName!,
        //        Email = user.Email!,
        //        Roles = roles,
        //        Claims = claims.Select(c => new ClaimDto { Type = c.Type, Value = c.Value }).ToList(),
        //        // Token bilgilerini de cevaba ekleyebiliriz. Bu, frontend'in ihtiyacına bağlı.
        //        // Genellikle token'lar ayrı bir nesne içinde döner, biz de öyle yapalım.
        //        // Bu yüzden UserLoginResponseDto'ya bir TokenResponseDto propertysi eklemek iyi olabilir.
        //        // Şimdilik ayrı tutuyorum. Frontend hem kullanıcı bilgisini hem de token'ı alacak.
        //    };

        //    // Yeni LoginSuccessDto'yu oluşturup hem token'ları hem de kullanıcı bilgilerini içine koyuyoruz.
        //    //BURAYI 13.soruda gelen cevaba ve aşağıdaki notlara göre düzenledim. loginsuccessdto oluşturdum. burada iki dto yu birleştiriyorum.
        //    var loginSuccessResponse = new LoginSuccessDto
        //    {
        //        Tokens = tokens,
        //        User = userResponse
        //    };



        //    // Login işleminin asıl çıktısı token'dır. Başarılı mesajı ile birlikte token'ları döndürebiliriz.
        //    // Ama interface'imiz UserLoginResponseDto bekliyor. Bu durumda, iki bilgiyi de içeren bir DTO daha mantıklı olabilir.
        //    // Mevcut DTO'ları bozmadan devam edelim, controller seviyesinde iki bilgiyi birleştirebiliriz.
        //    // Örnek: return new SuccessDataResult<object>(new { User = response, Token = tokens });
        //    // Şimdilik interface'e sadık kalalım. Token'ları RefreshTokenAsync'den alacaklarını varsayalım.
        //    // En iyi pratik: Login sonucu token'ları ve temel kullanıcı bilgilerini döndürmektir.
        //    // UserLoginResponseDto'ya TokenResponseDto'yu da ekleyelim.
        //    // public TokenResponseDto Tokens { get; set; } gibi. Şimdilik mevcut yapıyla devam ediyorum.
        //    // Controller'da iki ayrı çağrı yapılması gerekecek bu durumda. Bu verimsiz olur.
        //    // Login işleminin sonucunu değiştirelim ve hem token hem kullanıcı bilgisi döndürelim.
        //    // Yeni bir DTO oluşturmak en temizi:
        //    // public class LoginSuccessDto { UserLoginResponseDto User; TokenResponseDto Tokens; }
        //    // Şimdilik interface'e sadık kalalım ve sadece UserLoginResponseDto döndürelim. Token'lar için ayrı bir endpoint çağrısı gerektiğini varsayalım.
        //    // Düzeltme: En iyi yaklaşım, login'in token döndürmesidir. Interface'i buna göre düzenlemek daha mantıklı.
        //    // IAuthService -> Task<IDataResult<TokenResponseDto>> LoginAsync(LoginDto dto); olmalı.
        //    // Mevcut interface'i koruyarak devam ediyorum.
        //    return new SuccessDataResult<LoginSuccessDto?>(loginSuccessResponse, "Giriş başarılı.");
        //}

        //// Çıkış işlemi.
        //public async Task<IResult> LogoutAsync()
        //{
        //    // JWT tabanlı sistemlerde logout işlemi genellikle client-side'da token'ı silerek yapılır.
        //    // Ancak, refresh token'ı sunucu tarafında geçersiz kılabiliriz.
        //    // Bu metodun parametresiz olması, hangi refresh token'ı geçersiz kılacağımızı bilmediğimiz anlamına gelir.
        //    // Eğer o anki kullanıcıya ait tüm refresh token'ları geçersiz kılmak istersek, IHttpContextAccessor ile kullanıcı ID'sini alabiliriz.
        //    // Şimdilik en basit haliyle bırakıyorum.
        //    await _signInManager.SignOutAsync(); // Cookie tabanlı bir sistemle birlikte çalışıyorsa bu satır önemlidir.
        //    return new SuccessResult("Çıkış yapıldı.");
        //}

        //// Token yenileme işlemini TokenService'e delege eder.
        //public async Task<IDataResult<TokenResponseDto>> RefreshTokenAsync(string refreshToken)
        //{
        //    return await _tokenService.RefreshAccessTokenAsync(refreshToken);
        //}
    }
}
