using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.ExternalServices.Identity
{
    public class ClaimDto : IDto
    {
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}
