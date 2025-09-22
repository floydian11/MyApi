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


        //bu ctor dinamik mesajlar için. Yani mesaj içinde {0}, {1} gibi yer tutucular olabilir.
        //X ürünü silinemedi çünkü Y ile ilişkili gibi dinamik mesajlar için.
        // YENİ BİR CONSTRUCTOR EKLEYELİM veya mevcut olanı güncelleyelim.
        // En temizi, dinamik parametreleri kabul eden yeni bir constructor eklemektir.
        // Ancak basitlik için mevcut constructor'ı güncelleyelim:
        public Error(string code, string message, ErrorType type, params object[] args)
        {
            Code = code;
            // Eğer argüman varsa, mesajı formatla. Yoksa olduğu gibi kullan.
            Message = args.Length > 0 ? string.Format(message, args) : message;
            Type = type;
        }
    }
}
