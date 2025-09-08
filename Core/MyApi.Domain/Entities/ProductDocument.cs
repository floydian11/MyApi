using MyApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Domain.Entities
{
    public class ProductDocument : BaseEntity
    {
        public string FilePath { get; set; } = null!;
        public string FileName { get; set; } = null!;

        // Foreign Key
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
