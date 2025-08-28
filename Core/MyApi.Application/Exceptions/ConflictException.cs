using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message = "Çakışan veri") : base(message) { }
        //ConflictException → 409 (çakışan veri, ör. duplicate kayıt)
    }
}
