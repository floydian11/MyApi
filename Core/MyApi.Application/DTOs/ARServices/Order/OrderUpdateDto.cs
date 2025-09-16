using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.ARServices.Order
{
    public class OrderUpdateDto : IDto
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string? Notes { get; set; }
        public List<OrderItemUpdateDto> OrderItems { get; set; } = new();
    }
}
