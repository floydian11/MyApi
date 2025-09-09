using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.Order
{
    public class OrderCreateDto : IDto
    {
        public string CustomerName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; } = null!; // ileride enum olabilir
        public string? Notes { get; set; }

        public List<OrderItemCreateDto> OrderItems { get; set; } = new();
    }
}
