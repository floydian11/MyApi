using MediatR;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories.JWT;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Authentication.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetActiveTokenAsync(request.RefreshToken);

            if (refreshToken != null)
            {
                refreshToken.IsActive = false;
                refreshToken.RevokedDate = DateTime.UtcNow;
                _refreshTokenRepository.Update(refreshToken);
                await _unitOfWork.CommitAsync();
            }

            // Token bulunamasa bile, client zaten çıkış yapmış olduğu için başarılı dönebiliriz.
            // Bu bir hata durumu değildir.
            return Result.Success("Oturum başarıyla sonlandırıldı.");
        }
    }

    //Önemli Not: JWT'ler "stateless" (durumsuz) olduğu için,
    //AccessToken'ı sunucu tarafında "öldüremezsin".
    //Logout, client'ın token'ı silmesiyle olur.Bizim burada yapacağımız şey,
    //çalınmasını önlemek için RefreshToken'ı veritabanında geçersiz kılmaktır.
}
