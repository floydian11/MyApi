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
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Persistence.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
     : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductDetail> ProductDetails { get; set; } = null!;
        public DbSet<ProductDocument> ProductDocuments { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

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
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                var utcNow = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = utcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = utcNow;
                        break;
                    case EntityState.Deleted:
                        // Hard delete ise hiç bir şey yapma
                        // Soft delete ise entry.Entity.IsDeleted = true gibi işle
                        break;
                }
            }

            // AppUser için de aynı mantık
            foreach (var entry in ChangeTracker.Entries<AppUser>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.UtcNow;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

