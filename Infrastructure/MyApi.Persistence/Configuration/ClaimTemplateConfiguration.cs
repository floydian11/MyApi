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
    public class ClaimTemplateConfiguration : IEntityTypeConfiguration<ClaimTemplate>
    {
        public void Configure(EntityTypeBuilder<ClaimTemplate> builder)
        {
            builder.HasKey(ct => ct.Id);

            // Type ve Value birlikte benzersiz olmalıdır.
            // Yani, "permission" tipinde "products.delete" değerinden sadece bir tane olabilir.
            builder.HasIndex(ct => new { ct.Type, ct.Value }).IsUnique();

            builder.Property(ct => ct.Type).IsRequired().HasMaxLength(100);
            builder.Property(ct => ct.Value).IsRequired().HasMaxLength(100);
            builder.Property(ct => ct.Description).IsRequired().HasMaxLength(250);
        }
    }
}
