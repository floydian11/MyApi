using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.Product
{
    public class ProductResponseDto  : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsActive { get; set; }

        // ProductDetail alanları
        public string? Manufacturer { get; set; }
        public string? TechnicalSpecs { get; set; }
        public int? WarrantyPeriodInMonths { get; set; }

        // Kategori ID’leri
        public List<Guid> CategoryIds { get; set; } = new();

        // Product Image Path
        public string? ProductImagePath { get; set; }

        // Document Paths
        public List<string> ProductDocuments { get; set; } = new();
    }
}
