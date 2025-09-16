using Microsoft.AspNetCore.Http;

namespace MyApi.Application.DTOs.ARServices.Product
{
    public class ProductUpdateDto : IDto
    {
        public Guid Id { get; set; }  // Eklendi
        public string Name { get; set; } = null!; // zorunlu alan
        public string? Description { get; set; }
        public decimal? Price { get; set; } // null olabilir, update sırasında değişmeyebilir
        public DateTime? ReleaseDate { get; set; } // null olabilir
        public bool? IsActive { get; set; }

        // ProductDetail alanları
        public string? Manufacturer { get; set; }
        public string? TechnicalSpecs { get; set; }
        public int? WarrantyPeriodInMonths { get; set; }

        // Kategoriler
        public List<Guid> CategoryIds { get; set; } =  new List<Guid>();

        // Dosyalar
        public IFormFile? ProductImage { get; set; } // tekil resim
        public string? ImageFileName { get; set; }
        public List<IFormFile> Documents { get; set; } = new(); // çoklu dosya
    }
}
