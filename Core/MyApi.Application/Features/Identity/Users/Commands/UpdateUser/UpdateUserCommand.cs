using MediatR;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(
        Guid Id,//routedan gelen id bu (parametredeki id) alttakiler dtodaki alanlar. 
        string? Username,
        string? Email,
        string? FirstName,
        string? LastName,
        string? TCKN,
        bool? IsActive) : IRequest<Result<UserResponseDto>>;
}
