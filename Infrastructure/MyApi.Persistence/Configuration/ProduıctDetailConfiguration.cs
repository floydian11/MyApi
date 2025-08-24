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
    public class ProduıctDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            // Primary key
            builder.HasKey(d => d.Id);

            // Kolon ayarları
            builder.Property(d => d.Manufacturer)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(d => d.TechnicalSpecs)
                   .HasMaxLength(2000);

            builder.Property(d => d.WarrantyPeriodInMonths)
                   .IsRequired();

            // 1-1 ilişki: ProductDetail -> Product
            builder.HasOne(d => d.Product)
                   .WithOne(p => p.ProductDetail)
                   .HasForeignKey<ProductDetail>(d => d.ProductId) // FK dependent tabloda
                   .OnDelete(DeleteBehavior.Cascade);

            // UNIQUE constraint ile 1-1 garanti altına alınır
            builder.HasIndex(d => d.ProductId)
                   .IsUnique();
        }
    }
}
