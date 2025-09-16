using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Persistence.Configuration
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            // AppRole'daki özel alanlar için kurallar...
            builder.Property(x => x.Description).HasMaxLength(250);

            // İlişki yapılandırması
            builder.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role) // Her UserRole'ün bir rolü vardır.
                .HasForeignKey(ur => ur.RoleId) // Yabancı anahtar RoleId'dir.
                .IsRequired();
        }
    }
}
