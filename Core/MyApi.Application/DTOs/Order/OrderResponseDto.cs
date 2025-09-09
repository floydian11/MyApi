using MyApi.Application.DTOs.Product;
using MyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.Order
{
    public class OrderResponseDto : IDto
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string? Notes { get; set; }

        public List<OrderItemResponseDto> OrderItems { get; set; } = new();
    }
}
