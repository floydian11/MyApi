using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.Product
{
    public class ProductDetailDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsActive { get; set; }

        public ProductDetailInfoDto ProductDetail { get; set; } = null!;

        public List<ProductCategoryDto> Categories { get; set; } = new();

        public string? ProductImagePath { get; set; }
        public List<ProductDocumentDto> Documents { get; set; } = new();
    }
}
