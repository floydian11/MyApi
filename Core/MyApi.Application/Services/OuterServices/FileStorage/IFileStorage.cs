using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.OuterServices.FileStorage
{
    public interface IFileStorage
    {
        /// <summary>Tek bir dosya kaydeder</summary>
        Task<FileUploadResult> SaveFileAsync(FileUploadItem file, string folder);

        /// <summary>
        /// Tek veya çoklu dosyayı kaydeder.
        /// </summary>
        /// <param name="files">Upload edilecek dosya listesi</param>
        /// <param name="folder">Klasör adı (root sonrası)</param>
        /// <returns>Kaydedilen dosyaların bilgisi</returns>
        Task<List<FileUploadResult>> SaveFilesAsync(IEnumerable<FileUploadItem> files, string folder);

        /// <summary>
        /// Dosya siler
        /// </summary>
        /// <param name="path">Silinecek dosyanın path'i</param>
        /// <summary>Dosyayı siler ve sonucu döner</summary>
        Task<bool> DeleteFileAsync(string path);

        /// <summary>
        /// Dosya var mı kontrolü
        /// </summary>
        bool HasFile(IFormFile? file);

    }
}
