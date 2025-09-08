using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.Product
{
    public class ProductDocumentDto : IDto
    {
        public Guid Id { get; set; }
        public string FilePath { get; set; } = null!; 
        public string FileName { get; set; } = null!;
    }
}
