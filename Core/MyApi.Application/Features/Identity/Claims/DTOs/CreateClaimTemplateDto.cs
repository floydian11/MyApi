using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.DTOs
{
    //API'den Create için alınacak veri
    public record CreateClaimTemplateDto(
    string Type,
    string Value,
    string Description
);
}
