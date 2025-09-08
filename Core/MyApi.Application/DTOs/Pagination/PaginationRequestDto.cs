using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.Pagination
{
    public class PaginationRequestDto : IDto
    {
        public int Page { get; set; } = 1;       // 1’den başlar
        public int PageSize { get; set; } = 10;  // Varsayılan 10
    }
}
