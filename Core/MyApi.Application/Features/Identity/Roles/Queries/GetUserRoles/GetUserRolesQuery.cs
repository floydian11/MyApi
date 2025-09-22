using MediatR;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles.Queries.GetUserRoles
{
    // Bu sorgu, hangi kullanıcının rollerinin istendiğini belirtmek için bir UserId alır.
    // Sonuç olarak rol isimlerinin bir listesini (string listesi) dönecek.
    public record GetUserRolesQuery(Guid UserId) : IRequest<Result<IList<string>>>;
}
