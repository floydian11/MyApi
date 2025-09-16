using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.ARServices.Order
{
    public class OrderItemUpdateDto : IDto
    {
        public Guid Id { get; set; }  // güncellenecek satırın Id’si
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? Discount { get; set; }
    }
}
