using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Exceptions;
using MyApi.Domain.Entities;
using MyApi.Domain.Entities.Common;
using MyApi.Domain.Entities.Identity;
using MyApi.Domain.Entities.JWT;
using MyApi.Persistence.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Persistence.Context
{

    
    //public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    // DbContext'inize, her bir Identity tablosu için HANGİ sınıfı kullanacağını generic parametreler ile tek tek söylüyoruz.
    public class AppDbContext : IdentityDbContext<
    AppUser,                  // Kullanıcılar için AppUser sınıfını kullan
    AppRole,                  // Roller için AppRole sınıfını kullan
    Guid,                     // Primary Key tipi olarak Guid kullan
    IdentityUserClaim<Guid>,  // Claim'ler için standart sınıfı kullan
    AppUserRole,              // Kullanıcı-Rol ilişkisi için özel AppUserRole sınıfını kullan
    IdentityUserLogin<Guid>,  // Login'ler için standart sınıfı kullan
    IdentityRoleClaim<Guid>,  // Rol Claim'leri için standart sınıfı kullan
    IdentityUserToken<Guid>>  // Token'lar için standart sınıfı kullan    
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
     : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductDetail> ProductDetails { get; set; } = null!;
        public DbSet<ProductDocument> ProductDocuments { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<ClaimTemplate> ClaimTemplates { get; set; } = null!;
        //FILE SINIFLARI

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Identity tabloları ve ilişkileri için gerekli
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); // kendi entity konfigürasyonların

            // modelBuilder.ApplyConfiguration(new ProductConfiguration()); manuael taanımlama isteseydik buydu. ama yukarıdaki tüm configuration sınıflarımızı bulur ve buraya ekler. 

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.Discount)
                .HasPrecision(18, 2); // precision: toplam basamak, scale: ondalık basamak

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // O anki giriş yapmış kullanıcının ID'sini al. Claim'lerde "sub" veya "nameidentifier" olarak tutulur.
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            // IAuditableEntity arayüzünü uygulayan tüm entity'leri yakala.
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                var utcNow = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = utcNow;
                        entry.Entity.CreatedBy = userId; // Kullanıcı ID'sini ekle
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = utcNow;
                        entry.Entity.UpdatedBy = userId; // Kullanıcı ID'sini ekle
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

