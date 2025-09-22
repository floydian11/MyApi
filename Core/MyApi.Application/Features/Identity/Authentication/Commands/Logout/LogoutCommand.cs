using MediatR;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Authentication.Commands.Logout
{
    // Geçersiz kılınacak RefreshToken'ı girdi olarak alır. Geriye bir şey dönmez.
    public record LogoutCommand(string RefreshToken) : IRequest<Result>;
}
