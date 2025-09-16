using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.DTOs.ExternalServices.FileUpload 
{
    public class FileUploadItem : IDto
    {
        public IFormFile File { get; set; } = null!;   // Dosyanın kendisi
        public string FileName { get; set; } = null!;  // Kaydedilecek isim
        public string? ContentType { get; set; }       // MIME tipi, örn: "image/png"
        public string? Extension { get; set; }         // Dosya uzantısı, örn: ".jpg"
    }
}
