using MyApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Domain.Entities.Identity
{
    public class ClaimTemplate : BaseEntity
    {
        // Claim'in tipi, genellikle bir kategori belirtir. Örn: "permission"
        public string Type { get; set; } = null!;

        // Claim'in değeri, asıl yetki kodudur. Örn: "products.delete"
        public string Value { get; set; } = null!;

        // Bu yetkinin admin panelinde görünecek açıklaması. Örn: "Ürün Silme Yetkisi"
        public string Description { get; set; } = null!;

        // Soft delete ve pasif/aktif yapma için.
        public bool IsActive { get; set; } = true;
    }
}
