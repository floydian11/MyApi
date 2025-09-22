using MediatR;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Users.Queries.GetAllUsers
{
    public record GetAllUsersQuery(): IRequest<Result<List<UserResponseDto>>>;
    
}
