using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Persistence.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(p => p.Id);

            // 🏷️ Name: zorunlu, max 200 karakter
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            // 📄 Description: opsiyonel, max 1000 karakter
            builder.Property(p => p.Description)
                   .HasMaxLength(1000);

            // 💰 Price: zorunlu, decimal(18,2) tipinde (para için uygun)
            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            // 📅 ReleaseDate: zorunlu
            builder.Property(p => p.ReleaseDate)
                   .IsRequired();

            // ✅ IsActive: default olarak true
            builder.Property(p => p.IsActive)
                   .HasDefaultValue(true);

            // 🖼️ ProductImagePath: opsiyonel, max 500 karakter
            builder.Property(p => p.ProductImagePath)
                   .HasMaxLength(500);

            // 🔗 1-1 ilişki: Product ↔ ProductDetail
            // Bir Product'ın tam olarak bir detayı vardır.
            builder.HasOne(p => p.ProductDetail)
                   .WithOne(d => d.Product)
                   .HasForeignKey<ProductDetail>(d => d.ProductId) // ProductDetail tablosunda ProductId FK olacak
                   .OnDelete(DeleteBehavior.Cascade);              // Product silinirse detay da silinir

            // 🔗 1-n ilişki: Product ↔ ProductDocuments
            // Bir Product'ın birden fazla dokümanı olabilir.
            builder.HasMany(p => p.ProductDocuments)
                   .WithOne(d => d.Product)
                   .HasForeignKey(d => d.ProductId)                // ProductDocument tablosunda ProductId FK olacak
                   .OnDelete(DeleteBehavior.Cascade);              // Product silinirse dokümanlar da silinir

            // 🔗 m-n ilişki: Product ↔ Category
            // Bir ürün birden fazla kategoriye, bir kategori birden fazla ürüne ait olabilir.
            builder.HasMany(p => p.Categories)
                   .WithMany(c => c.Products)
                   .UsingEntity<Dictionary<string, object>>(        // Join tablosunu dictionary ile oluşturuyoruz
                        "ProductCategory",                          // Join tablosunun adı
                        j => j.HasOne<Category>()                   // Join tablosundan Category'e giden ilişki
                              .WithMany()
                              .HasForeignKey("CategoryId")
                              .OnDelete(DeleteBehavior.Cascade),
                        j => j.HasOne<Product>()                    // Join tablosundan Product'a giden ilişki
                              .WithMany()
                              .HasForeignKey("ProductId")
                              .OnDelete(DeleteBehavior.Cascade),
                        j =>
                        {
                            j.HasKey("ProductId", "CategoryId");    // Composite key: (ProductId + CategoryId)
                            j.ToTable("ProductCategories");         // Join tablosunun adı override edildi
                        });
        }
    }

}
