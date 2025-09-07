using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.OuterServices.FileStorage
{
    public class FileUploadResult
    {
        public string FileName { get; set; } = null!;  // Kaydedilen dosya adı
        public string FilePath { get; set; } = null!;  // Fiziksel veya sanal path
        public string? ContentType { get; set; }       // MIME tipi
        public long FileSize { get; set; }             // Dosya boyutu (opsiyonel ama faydalı)
    }
}
