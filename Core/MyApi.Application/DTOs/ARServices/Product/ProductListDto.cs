using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.ARServices.Product
{
    public class ProductListDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string? ProductImagePath { get; set; }

        // Yeni eklenen alan
        public List<string> CategoryNames { get; set; } = new();
        public List<string> DocumentPaths { get; set; } = new();
    }
}
