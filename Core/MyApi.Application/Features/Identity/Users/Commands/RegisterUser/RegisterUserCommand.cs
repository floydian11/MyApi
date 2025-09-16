using MediatR; // MediatR kütüphanesini dahil ediyoruz.
using MyApi.Application.Features.Identity.Users.DTOs; // UserResponseDto için.
using MyApi.Application.Results; // Kendi yazdığımız Result deseni için.

namespace MyApi.Application.Features.Identity.Users.Commands.RegisterUser
{
    // Bu bir 'Command'dir ve 'record' olarak tanımlanması çok yaygındır çünkü temelde bir veri taşıyıcısıdır.
    // IRequest<T> arayüzünü uygular. Bu, MediatR'a "Bu komut işlendiğinde,
    // geriye bir Result<UserResponseDto> dönecek" demenin yoludur.
    public record RegisterUserCommand(
        string Username,
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string TCKN) : IRequest<IDataResult<UserResponseDto>>;
}
