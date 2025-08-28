using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "Yetkisiz erişim. Lütfen giriş yapınız.") : base(message) { }

       // ForbiddenException → 403 (rol veya yetki yok)

        //UnauthorizedException → 401 (kullanıcı login değil veya token geçersiz) 
    }

}
