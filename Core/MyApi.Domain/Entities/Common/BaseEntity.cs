using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Domain.Entities.Common
{
    public abstract class BaseEntity : IAuditableEntity
    {
        // set metodu artık private. Sadece nesne oluşturulurken atanabilir.
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
