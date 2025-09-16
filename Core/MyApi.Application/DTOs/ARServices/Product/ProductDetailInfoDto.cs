using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.ARServices.Product
{
    public class ProductDetailInfoDto : IDto
    {
        public string? Manufacturer { get; set; } = null!;
        public string? TechnicalSpecs { get; set; }
        public int? WarrantyPeriodInMonths { get; set; }
    }
}
