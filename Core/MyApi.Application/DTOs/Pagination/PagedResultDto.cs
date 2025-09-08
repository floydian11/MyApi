using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.Pagination
{
    public class PagedResultDto<T> : IDto
    {
        public List<T> Items { get; set; } = new(); // Sayfalanmış veri listesi
        public int TotalCount { get; set; } // Tüm kayıtların toplam sayısı (pagination için lazım)
        public int Page { get; set; } // O anki sayfa numarası (kullanıcıdan gelir)
        public int PageSize { get; set; }  // Sayfada kaç kayıt olacağı (kullanıcıdan gelir)
    }
}
