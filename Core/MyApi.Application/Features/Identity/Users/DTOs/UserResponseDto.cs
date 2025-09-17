using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Users.DTOs
{
    // Bu DTO, bir kullanıcı hakkında temel bilgileri temsil eder.
    // 'record' kullanıyoruz çünkü DTO'lar genellikle sadece veri taşır ve değişmez olmaları tercih edilir.
    // 'init' anahtar kelimesi, property'lerin sadece nesne oluşturulurken atanabileceği anlamına gelir.
    public record UserResponseDto
    {
        public Guid Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;

        // Koleksiyonları da 'init' ile tanımlayabiliriz.
        // Constructor'da boş bir liste ile başlatmak, 'NullReferenceException' hatalarını önler.
        public IEnumerable<string> Roles { get; init; } = new List<string>();
        public IEnumerable<ClaimDto> Claims { get; init; } = new List<ClaimDto>();
    }
}
