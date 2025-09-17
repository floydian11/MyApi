using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Users.DTOs
{
    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? TCKN { get; set; }  // Opsiyonel, hash servisi ile işlenecek
        public bool? IsActive { get; set; } // Opsiyonel, admin update için
    }
}
