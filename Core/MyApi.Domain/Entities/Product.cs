using MyApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsActive { get; set; }

        // 1-1 ilişki
        public ProductDetail ProductDetail { get; set; } = null!;

        // 1-n ilişki
        public ICollection<ProductDocument> ProductDocuments { get; set; } = new List<ProductDocument>();

        // m-n ilişki
        public ICollection<Category> Categories { get; set; } = new List<Category>();

        // Tek dosya
        public string? ProductImagePath { get; set; }
    }
}
