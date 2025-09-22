using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Authentication
{
    public static class AuthenticationErrors
    {
        public static readonly Error RefreshFailed = new (
        "Token.RefreshFailed",
        "Token yenileme hatası.",
        ErrorType.Failure);
    }
}
