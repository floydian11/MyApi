using MyApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Domain.Entities
{
    public class ProductDetail : BaseEntity
    {
        public string Manufacturer { get; set; } = null!;
        public string? TechnicalSpecs { get; set; }
        public int WarrantyPeriodInMonths { get; set; }

        // Foreign key
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
