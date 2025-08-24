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
            // Primary key
            builder.HasKey(p => p.Id);

            // Kolon ayarları
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.Description)
                   .HasMaxLength(1000);

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.ReleaseDate)
                   .IsRequired();

            builder.Property(p => p.IsActive)
                   .HasDefaultValue(true);

            builder.Property(p => p.ProductImagePath)
                   .HasMaxLength(500);

            // 1-1 ilişki: Product -> ProductDetail
            builder.HasOne(p => p.ProductDetail)
                   .WithOne(d => d.Product)
                   .HasForeignKey<ProductDetail>(d => d.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 1-n ilişki: Product -> ProductDocuments
            builder.HasMany(p => p.ProductDocuments)
                   .WithOne(d => d.Product)
                   .HasForeignKey(d => d.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            // m-n ilişki: Product <-> Category
            builder.HasMany(p => p.Categories)
                   .WithMany(c => c.Products)
                   .UsingEntity<Dictionary<string, object>>(
                        "ProductCategory", // join tablosunun adı
                        j => j.HasOne<Category>()
                              .WithMany()
                              .HasForeignKey("CategoryId")
                              .OnDelete(DeleteBehavior.Cascade),
                        j => j.HasOne<Product>()
                              .WithMany()
                              .HasForeignKey("ProductId")
                              .OnDelete(DeleteBehavior.Cascade),
                        j =>
                        {
                            j.HasKey("ProductId", "CategoryId");
                            j.ToTable("ProductCategories");
                        });
        }
    }
}
