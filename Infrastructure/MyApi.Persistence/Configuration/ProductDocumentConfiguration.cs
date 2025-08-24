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
    public class ProductDocumentConfiguration : IEntityTypeConfiguration<ProductDocument>
    {
        public void Configure(EntityTypeBuilder<ProductDocument> builder)
        {
            // Primary key
            builder.HasKey(d => d.Id);

            // FilePath kolon ayarı
            builder.Property(d => d.FilePath)
                   .IsRequired()
                   .HasMaxLength(500);

            // 1-n ilişki: Product -> ProductDocuments
            builder.HasOne(d => d.Product)          // Her doküman bir ürüne ait
                   .WithMany(p => p.ProductDocuments) // Her ürünün birden fazla dokümanı olabilir
                   .HasForeignKey(d => d.ProductId)   // FK ProductDocument tablosunda
                   .OnDelete(DeleteBehavior.Cascade); // Product silinirse dokümanlar da silinir
        }
    }
    
}
