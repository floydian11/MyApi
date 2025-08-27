using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.Category
{
    public class CategoryUpdateDto : IDto
    {
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
        public bool IsAdminAction { get; set; } = false; // İş kuralı
    }
}
