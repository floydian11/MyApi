using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.DTOs
{
    public record ClaimDto
    {
        public string Type { get; init; } = string.Empty;
        public string Value { get; init; } = string.Empty;
    }
}
