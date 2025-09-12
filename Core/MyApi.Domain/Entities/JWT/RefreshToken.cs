using MyApi.Domain.Entities.Common;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Domain.Entities.JWT
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? RevokedDate { get; set; }

        // Navigation property
        public AppUser User { get; set; } = null!;
    }
}
