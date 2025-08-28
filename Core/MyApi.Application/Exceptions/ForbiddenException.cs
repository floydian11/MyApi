using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message = "Bu alana giriş yetkiniz yoktur") : base(message) { }
        //ForbiddenException 403 Kullanıcı yetkisi yok


    }
}
