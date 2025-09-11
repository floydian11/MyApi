using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Domain.Entities.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        public string? Description { get; set; }
    }
}
