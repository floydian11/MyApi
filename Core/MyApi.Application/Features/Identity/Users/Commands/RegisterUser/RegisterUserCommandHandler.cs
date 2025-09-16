using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Results;
using MyApi.Application.Services.OuterServices.Identity.Hash;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyApi.Application.Features.Identity.Users.Commands.RegisterUser
{
    // IRequestHandler<TCommand, TResponse> arayüzünü uygular.
    // Bu, MediatR'a "Ben RegisterUserCommand'i işlerim ve Result<UserResponseDto> döndürürüm" der.
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IDataResult<UserResponseDto>>
    {
        // Bu Handler'ın SADECE bu iş için ihtiyacı olan bağımlılıkları enjekte ediyoruz.
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;

        public RegisterUserCommandHandler(UserManager<AppUser> userManager, IMapper mapper, IHashService hashService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _hashService = hashService;
        }
        // MediatR'ın çağıracağı ana metod budur. 'request' parametresi, gönderilen command'in kendisidir.
        public async Task<IDataResult<UserResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // 1. İş Kuralı: Kullanıcı adı daha önce alınmış mı?
            var existingUser = await _userManager.FindByNameAsync(request.Username);
            
            if (existingUser != null)
            {
                return new ErrorDataResult<UserResponseDto>(null, "Bu kullanıcı adı zaten kullanılıyor.");
            }
            // 2. İş Kuralı: E-posta daha önce alınmış mı?
            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail != null)
            {
                return new ErrorDataResult<UserResponseDto>(null, "Bu e-posta adresi zaten kullanılıyor.");
            }

            // 3. Command'den gelen veriyi AppUser entity'sine map'le.
            var newUser = _mapper.Map<AppUser>(request);

            // 4. TCKN'i hash'le ve entity'e ata.
            var (tcknHash, tcknSalt) = _hashService.HashValue(request.TCKN);
            newUser.TCKNHash = tcknHash;
            newUser.TCKNSalt = tcknSalt;

            // 5. Identity'nin UserManager'ını kullanarak kullanıcıyı ve şifresini (hash'lenmiş olarak) yarat.
            var identityResult = await _userManager.CreateAsync(newUser, request.Password);

            // 6. Identity'den dönen sonuç başarılı değilse, hatayı dön.
            if (!identityResult.Succeeded)
            {
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                return new ErrorDataResult<UserResponseDto>(null, $"Kullanıcı oluşturulamadı: {errors}");
            }

            // 7. Yeni kullanıcıya varsayılan "User" rolünü ata.
            await _userManager.AddToRoleAsync(newUser, "User");

            // 8. Oluşturulan kullanıcıyı, API'ye döneceğimiz UserResponseDto'ya map'le.
            var userResponse = _mapper.Map<UserResponseDto>(newUser);
            // Roller manuel olarak eklenebilir, çünkü henüz navigation property'ler yüklenmemiş olabilir.
            userResponse = userResponse with { Roles = new List<string> { "User" } };

            // 9. Her şey başarılı ise, Success Result'ı ve map'lenmiş DTO'yu dön.
            return new SuccessDataResult<UserResponseDto>(userResponse, "Kullanıcı başarıyla kaydedildi.");
        }
    }
}
//WITH KULLANIMI
//"userResponse nesnesinin bir kopyasını oluştur.
//Bu kopyada diğer tüm property'ler (Id, Username vb.) orijinaliyle aynı kalsın,
//ama Roles property'sinin değerini new List<string> { "User" } olarak ayarla.
//Sonra bu yeni oluşturduğun kopyayı al ve userResponse değişkenine ata."