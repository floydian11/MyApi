using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyApi.Application.DTOs.Product
{
    public class ProductCreateDto : IDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsActive { get; set; }

        // ProductDetail bilgileri
        public string Manufacturer { get; set; } = null!;
        public string? TechnicalSpecs { get; set; }
        public int WarrantyPeriodInMonths { get; set; }

        // Categories
        public List<Guid> CategoryIds { get; set; } = new();

        // Resim
        public IFormFile? ProductImage { get; set; }
        public string? ImageFileName { get; set; }

        // Belgeler
        public List<IFormFile> Documents { get; set; } = new();
    }
}
