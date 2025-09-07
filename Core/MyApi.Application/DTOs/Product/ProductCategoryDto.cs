using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.Product
{
    public class ProductCategoryDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
