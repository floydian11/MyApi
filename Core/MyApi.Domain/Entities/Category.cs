using MyApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;

        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public bool IsActive { get; set; } = true;
        public bool IsAdminAction { get; set; } = false;
    }
    // Junction tablosu ile ilişki
     // public ICollection<ProductCategories> ProductCategories { get; set; } = new List<ProductCategories>();
}
