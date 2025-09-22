using MediatR;
using MyApi.Application.Features.Identity.Roles.DTOs;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles.Commands.UpdateRole
{
    // Bir rolü güncellemek için gereken tüm bilgileri içeren bir 'record'.
    // Başarılı olduğunda, güncellenmiş rolün DTO'sunu döndüreceğini belirtir.
    public record UpdateRoleCommand(
        Guid Id,             // Hangi rolün güncelleneceğinin kimliği
        string Name,         // Rolün yeni programatik adı (örn: "Product.Update")
        string? Description  // Rolün yeni görünen adı (örn: "Ürün Güncelleme Yetkisi")
    ) : IRequest<Result<RoleDto>>;
}
