using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.ExternalServices.Account
{
    public class RegisterDto : IDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string TCKN { get; set; } = null!; //TCKN için plain text olarak DTO alıyoruz, servis içinde hash + salt’a çevirilecek
    }
}
