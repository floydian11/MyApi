using MyApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Domain.Entities
{
    public class ProductDetail
    {
        public Guid ProductId { get; set; }   // Hem PK hem FK

        public string? Manufacturer { get; set; } = null!;
        public string? TechnicalSpecs { get; set; } = null!;
        public int WarrantyPeriodInMonths { get; set; }
        // Navigation
        public Product Product { get; set; } = null!;
    }
}
