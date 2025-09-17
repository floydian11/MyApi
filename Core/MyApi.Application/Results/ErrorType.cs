using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Results
{
    public enum ErrorType
    {
        Failure,      // Genel hata (500 Internal Server Error)
        Validation,   // Doğrulama hatası (400 Bad Request)
        NotFound,     // Kaynak bulunamadı (404 Not Found)
        Conflict      // Çakışma, örn: kayıt zaten var (409 Conflict)
    }
}
