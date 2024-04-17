using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace presentationLayer.Helpers
{
    public static class FileSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            string folderPath =
                Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            string filePath = Path.Combine(folderPath, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            return fileName;
        }

        public static bool DeleteFile(string fileName, string folderName)
        {
            bool deleted = false;
            string filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                @"wwwroot\files",
                folderName,
                fileName
                );
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                deleted = true;
            }
            return deleted;
        }
    }
}
