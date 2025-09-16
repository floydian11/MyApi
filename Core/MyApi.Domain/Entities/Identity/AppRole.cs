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
        // Constructor metodu ekliyoruz
        public AppRole()
        {
            // UserRoles koleksiyonunu boş bir koleksiyon olarak başlatıyoruz.
            UserRoles = new HashSet<AppUserRole>();
        }
        public string? Description { get; set; }
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
       
    }
}
