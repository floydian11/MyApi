using Microsoft.AspNetCore.Http;
using MyApi.Application.Services.OuterServices.FileStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Infrastructure.FileStorage
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly string _rootPath;
        public LocalFileStorage(string rootPath)
        {
            _rootPath = rootPath;
        }

        public async Task<FileUploadResult> SaveFileAsync(FileUploadItem file, string folder)
        {
            if (!HasFile(file.File))
                throw new ArgumentException("Dosya geçersiz.");

            var folderPath = Path.Combine(_rootPath, folder);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var fullPath = Path.Combine(folderPath, uniqueFileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.File.CopyToAsync(stream);

            return new FileUploadResult
            {
                FileName = file.FileName,
                FilePath = Path.Combine(folder, uniqueFileName).Replace("\\", "/"),
                ContentType = file.File.ContentType
            };
        }


        public async Task<List<FileUploadResult>> SaveFilesAsync(IEnumerable<FileUploadItem> files, string folder)
        {
            var results = new List<FileUploadResult>();

            foreach (var file in files)
            {
                var result = await SaveFileAsync(file, folder);
                results.Add(result);
            }

            return results;
        }

        public async Task<bool> DeleteFileAsync(string path)
        {
            var fullPath = Path.Combine(_rootPath, path);
            return await Task.Run(() =>
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            });
        }

        // Dosya var mı kontrol
        public bool HasFile(IFormFile? file)
        {
            return file != null && file.Length > 0;
        }

        
    }
}
