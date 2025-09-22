using MediatR;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid Id):IRequest<Result>;
}
