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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            
            // OnModelCreating'den taşıdığımız kod
            builder.Property(oi => oi.Discount)
                   .HasPrecision(18, 2);

            // Order ile olan ilişkisini de burada tanımlamak en doğrusudur.
            // Bir Order'ın çok sayıda OrderItem'ı olabilir.
            //builder.HasOne(oi => oi.Order)
            //       .WithMany(o => o.Items) // Order sınıfındaki koleksiyon property'si
            //       .HasForeignKey(oi => oi.OrderId); // OrderItem'daki foreign key
        }
    }
}
