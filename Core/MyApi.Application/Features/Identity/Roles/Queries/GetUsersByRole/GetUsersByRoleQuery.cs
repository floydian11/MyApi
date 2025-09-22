using MediatR;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles.Queries.GetUsersByRole
{
    // Bu sorgu, hangi kullanıcının rollerinin istendiğini belirtmek için bir UserId alır.
    // Sonuç olarak rol isimlerinin bir listesini (string listesi) dönecek.
    public record GetUsersByRoleQuery(string Name) : IRequest<Result<IList<UserListDto>>>;
}
