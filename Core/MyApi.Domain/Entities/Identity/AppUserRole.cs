using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Domain.Entities.Identity
{
    public class AppUserRole : IdentityUserRole<Guid>
    {
        public virtual AppUser User { get; set; } = null!;
        public virtual AppRole Role { get; set; } = null!;
    }
}
