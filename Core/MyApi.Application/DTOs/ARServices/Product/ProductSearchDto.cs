using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.ARServices.Product
{
    public class ProductSearchDto : IDto
    {
        public string? SearchTerm { get; set; }  // Arama yapılacak anahtar kelime
        public int Page { get; set; } = 1;       // Sayfa numarası, default 1
        public int PageSize { get; set; } = 10;  // Sayfa başına kayıt sayısı, default 10
    }
}
