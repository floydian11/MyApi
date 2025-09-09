using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.Order
{
    public class OrderItemResponseDto : IDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!; // mapping ile Product’tan alınır
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } // mapping ile Product.Price
        public decimal? Discount { get; set; }
    }
}
