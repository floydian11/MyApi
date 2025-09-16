using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.ARServices.Order
{
    public class OrderItemResponseDto : IDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Discount { get; set; }

        // Hesaplanan alanlar
        public decimal Total => Quantity * UnitPrice;
        public decimal TotalWithDiscount => Discount.HasValue
                                            ? Total - Total * Discount.Value / 100
                                            : Total;
    }
}
