using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.ARServices.Category
{
    public class CategoryCreateDto : IDto
    {
        public string Name { get; set; } = null!; // Boş olamaz → validation ile kontrol edilecek
        public bool IsActive { get; set; } = true;
    }
}
