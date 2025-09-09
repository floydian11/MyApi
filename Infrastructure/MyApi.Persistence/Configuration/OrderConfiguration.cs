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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            // 🔑 Primary Key
            builder.HasKey(p => p.Id);

            // 🏷️ Name: zorunlu, max 200 karakter
            builder.Property(p => p.CustomerName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.OrderDate)
                  .IsRequired();

            builder.Property(p => p.PaymentMethod)
                   .IsRequired()
                   .HasMaxLength(80);

            builder.Property(p => p.Notes)
                   .HasMaxLength(280);

            builder.HasMany(p => p.Items)
                   .WithOne(d => d.Order)
                   .HasForeignKey(d => d.OrderId)                // OrderItems tablosunda OrderId FK olacak
                   .OnDelete(DeleteBehavior.Cascade);              // Order silinirse dokümanlar da silinir

           
        }
    }
}
