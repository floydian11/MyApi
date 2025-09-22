using MediatR;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Authentication.Queries.LoginUser
{
    public record LoginQuery(
        string Username,
        string Password) :  IRequest<Result<LoginSuccessDto>>;
}
