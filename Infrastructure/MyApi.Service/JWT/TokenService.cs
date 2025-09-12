using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyApi.Application.Configuration.JWT;
using MyApi.Application.DTOs.Account;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories.JWT;
using MyApi.Application.Results;
using MyApi.Domain.Entities.Identity;
using MyApi.Domain.Entities.JWT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Infrastructure.JWT
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(IOptions<JwtSettings> jwtSettings, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
        {
            _jwtSettings = jwtSettings.Value;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TokenResponseDto> GenerateTokensAsync(AppUser user, IEnumerable<string> roles, IEnumerable<Claim> claims)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            // Rolleri ekle
            authClaims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            // Custom claim’ler varsa ekle
            authClaims.AddRange(claims);

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = GenerateRefreshToken(), // güvenli rastgele string
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.CommitAsync();

            TokenResponseDto tokenResponseDto = new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo
            };

            return tokenResponseDto; // ✅ Task sarmalı

        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<IDataResult<TokenResponseDto>> RefreshAccessTokenAsync(string refreshToken)
        {
            // 1️⃣ DB’den refresh token kontrolü (repo üzerinden)
            var tokenEntity = await _refreshTokenRepository
                .GetWhere(rt => rt.Token == refreshToken)
                .Include(rt => rt.User)
                .FirstOrDefaultAsync();

            if (tokenEntity == null || tokenEntity.ExpiresAt < DateTime.UtcNow)
                return new ErrorDataResult<TokenResponseDto>("Invalid or expired refresh token");

            var user = tokenEntity.User;

            // 2️⃣ Kullanıcının rolleri ve claim’leri (repo üzerinden)
            var roles = await _userRoleRepository
                .GetWhere(ur => ur.UserId == user.Id)
                .Select(ur => ur.Role.Name)
                .ToListAsync();

            var claims = await _userClaimRepository
                .GetWhere(uc => uc.UserId == user.Id)
                .Select(uc => new Claim(uc.ClaimType, uc.ClaimValue))
                .ToListAsync();

            // 3️⃣ Yeni access token üret
            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName ?? ""),
        new Claim(ClaimTypes.Email, user.Email ?? "")
    };
            authClaims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            authClaims.AddRange(claims);

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // 4️⃣ Refresh token’ı yenile (repo + unit of work)
            tokenEntity.Token = GenerateRefreshToken();
            tokenEntity.ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

            _refreshTokenRepository.Update(tokenEntity);
            await _unitOfWork.CommitAsync();

            // 5️⃣ TokenResponseDto döndür
            var tokenResponseDto = new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = tokenEntity.Token,
                ExpiresAt = token.ValidTo
            };

            return new SuccessDataResult<TokenResponseDto>(tokenResponseDto, "Token refreshed successfully");
        }
    }
}
