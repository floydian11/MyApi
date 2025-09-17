using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Results
{
    // Hata nesnemiz. Artık sadece bir string değil.
    public class Error
    {
        // Makine tarafından okunabilir, dile bağımsız bir kod. Örn: "User.NotFound"
        public string Code { get; }

        // Geliştirici için varsayılan, açıklayıcı mesaj.
        public string Message { get; }

        // Hatanın türü.
        public ErrorType Type { get; }

        public Error(string code, string message, ErrorType type)
        {
            Code = code;
            Message = message;
            Type = type;
        }
    }
}
