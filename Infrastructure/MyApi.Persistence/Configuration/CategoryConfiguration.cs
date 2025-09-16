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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            // n-n Category <-> Product PRODUCTTA ZATEN YAPTIK TEKRAR GEREKSİZ
            //builder.HasMany(c => c.Products)
            //       .WithMany(p => p.Categories)
            //       .UsingEntity<Dictionary<string, object>>(
            //            "ProductCategory",
            //            j => j.HasOne<Product>()
            //                  .WithMany()
            //                  .HasForeignKey("ProductId")
            //                  .OnDelete(DeleteBehavior.Cascade),
            //            j => j.HasOne<Category>()
            //                  .WithMany()
            //                  .HasForeignKey("CategoryId")
            //                  .OnDelete(DeleteBehavior.Cascade),
            //            j =>
            //            {
            //                j.HasKey("ProductId", "CategoryId");
            //                j.ToTable("ProductCategories");
            //            });
        }
    }
}
