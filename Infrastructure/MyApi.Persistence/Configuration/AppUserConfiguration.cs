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
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // AppUser'daki özel alanlar için kurallar burada tanımlanabilir.
            // Örnek: FirstName ve LastName zorunlu ve max uzunlukta olsun.
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);

            // Önceki adımda OnModelCreating'e yazdığımız İLİŞKİ yapılandırmaları buraya taşınacak.
            // Her kullanıcının birden fazla UserRole'ü olabilir.
            builder.HasMany(e => e.UserRoles)
                .WithOne(e => e.User) // Her UserRole'ün bir kullanıcısı vardır.
                .HasForeignKey(ur => ur.UserId) // Yabancı anahtar UserId'dir.
                .IsRequired();

            // Her kullanıcının birden fazla UserClaim'i olabilir.
            builder.HasMany(e => e.UserClaims)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();
        }
    }
}
