using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.Abstract
{
    public interface IHashService
    {
        (byte[] Hash, byte[] Salt) HashValue(string value);
        bool VerifyHash(string value, byte[] salt, byte[] hash);
    }
}
