using Microsoft.AspNetCore.Identity;
using MyApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<Guid>, IAuditableEntity
    {
        public AppUser()
        {
            UserRoles = new HashSet<AppUserRole>();
            UserClaims = new HashSet<IdentityUserClaim<Guid>>();
        }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public bool IsActive { get; set; } = true;

       // TC Kimlik No
       public byte[]? TCKNHash { get; set; }
       public byte[]? TCKNSalt { get; set; }

        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        public virtual ICollection<IdentityUserClaim<Guid>> UserClaims { get; set; }
       
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
   
}
