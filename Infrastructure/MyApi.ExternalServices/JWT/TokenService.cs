using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyApi.Application.DTOs.ExternalServices.Account;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories.JWT;
using MyApi.Application.Results;
using MyApi.Application.Services.OuterServices.Identity.JWT;
using MyApi.Domain.Entities.Identity;
using MyApi.Domain.Entities.JWT;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace MyApi.ExternalServices.JWT
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager; // Identity kullanıcı yönetimi
        private readonly IMapper _mapper;

        public TokenService(IOptions<JwtSettings> jwtSettings, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, 
            UserManager<AppUser> userManager, IMapper mapper)
        {
            _jwtSettings = jwtSettings.Value;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        // Access Token ve Refresh Token üretir.
        public async Task<TokenResponseDto> GenerateTokensAsync(AppUser user, IEnumerable<string> roles, IEnumerable<Claim> claims)
        {
            // 1. Access Token'ı oluştur.
            // Token'ın içereceği bilgileri (claim'leri) hazırlıyoruz.
            var tokenClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // 'Subject' - Token'ın sahibi olan kullanıcının ID'si.
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // 'JWT ID' - Her token için benzersiz bir kimlik.
                new Claim(ClaimTypes.Name, user.UserName!),             // Standart: Kullanıcı adı
                new Claim(ClaimTypes.Email, user.Email!),               // Standart: E-posta
                new Claim(ClaimTypes.GivenName, user.FirstName!),       // Standart: Ad
                 new Claim(ClaimTypes.Surname, user.LastName!),         // Standart: Soyad
            };

            // Parametre olarak gelen rolleri ve claim'leri de token'a ekliyoruz.
            tokenClaims.AddRange(claims);
            foreach (var role in roles)
            {
                tokenClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // appsettings.json'daki SecretKey'i byte dizisine çeviriyoruz. Bu anahtar, token'ı imzalamak için kullanılır.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            // Token'ın son kullanma tarihini hesaplıyoruz.
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

            // Token'ın dijital imzasını oluşturuyoruz. HMAC-SHA256 algoritmasını kullanıyoruz.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // JwtSecurityToken nesnesini, tüm bu bilgilerle (issuer, audience, claims, expires, credentials) oluşturuyoruz.
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: tokenClaims,
                expires: expires,
                signingCredentials: creds
            );

            // JwtSecurityTokenHandler sınıfı, token nesnesini string formatına çevirir.
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // 2. Refresh Token'ı oluştur ve veritabanına kaydet.
            var refreshTokenValue = GenerateRefreshToken();
            var refreshTokenExpires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenValue,
                ExpiresAt = refreshTokenExpires,
                IsActive = true
            };

            // Oluşturulan refresh token'ı veritabanına ekliyoruz.
            await _refreshTokenRepository.AddAsync(refreshToken);
            // Değişikliği kaydediyoruz.
            await _unitOfWork.CommitAsync();

            // 3. Sonucu DTO olarak döndür.
            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = expires
            };
        }
        // Yeni bir Refresh Token üretmek için kullanılır.
        public string GenerateRefreshToken()
        {
            // Kriptografik olarak güvenli, rastgele bir byte dizisi oluşturuyoruz.
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            // Bu byte dizisini Base64 string formatına çevirerek token'ı oluşturuyoruz.
            return Convert.ToBase64String(randomNumber);
        }
        
        // Süresi dolmuş bir Access Token'ı, geçerli bir Refresh Token ile yenilemek için kullanılır.
        public async Task<IDataResult<TokenResponseDto>> RefreshAccessTokenAsync(string refreshToken)
        {
            // Gelen refresh token veritabanında var mı ve aktif mi diye kontrol ediyoruz.
            var existingToken = await _refreshTokenRepository.GetActiveTokenAsync(refreshToken);

            // Kontrol 1: Token veritabanında bulunamadı veya artık aktif değil.
            if (existingToken == null)
            {
                return new ErrorDataResult<TokenResponseDto>(default, "Geçersiz veya süresi dolmuş refresh token.");
            }

            // Kontrol 2: Token'ın süresi dolmuş mu?
            if (existingToken.ExpiresAt < DateTime.UtcNow)
            {
                return new ErrorDataResult<TokenResponseDto>(default, "Refresh token'ın süresi dolmuş.");
            }

            // Token'ın sahibi olan kullanıcıyı buluyoruz.
            var user = await _userManager.FindByIdAsync(existingToken.UserId.ToString());
            if (user == null)
            {
                return new ErrorDataResult<TokenResponseDto>(default, "Kullanıcı bulunamadı.");
            }

            // ÖNEMLİ GÜVENLİK ADIMI: Refresh token'lar tek kullanımlık olmalıdır.
            // Kullandığımız eski token'ı pasif hale getiriyoruz.
            existingToken.IsActive = false;
            existingToken.RevokedDate = DateTime.UtcNow;
            _refreshTokenRepository.Update(existingToken);
            // Değişikliği hemen veritabanına yansıtıyoruz.
            await _unitOfWork.CommitAsync();

            // Kullanıcı için yeni rolleri ve claim'leri alıyoruz (yetkileri değişmiş olabilir).
            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);

            // Yeni bir Access Token ve yeni bir Refresh Token üretiyoruz.
            var newTokens = await GenerateTokensAsync(user, userRoles, userClaims);

            return new SuccessDataResult<TokenResponseDto>(newTokens, "Token başarıyla yenilendi.");
        }
    }
}
