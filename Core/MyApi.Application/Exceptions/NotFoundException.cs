using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "Aradığını kayıt bulunamadı") : base(message) { }
        //NotFoundException 404 Veri bulunamadı

    }

}
