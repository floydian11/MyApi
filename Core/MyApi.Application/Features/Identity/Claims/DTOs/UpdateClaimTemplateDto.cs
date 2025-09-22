using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.DTOs
{// UpdateClaimTemplateDto.cs (API'den Update için alınacak veri)
    public record UpdateClaimTemplateDto(
    string Type,
    string Value,
    string Description
);
}
