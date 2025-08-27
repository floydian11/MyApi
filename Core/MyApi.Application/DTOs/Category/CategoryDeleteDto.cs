using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.Category
{
    public class CategoryDeleteDto : IDto
    {
        public Guid Id { get; set; }
    }
}
