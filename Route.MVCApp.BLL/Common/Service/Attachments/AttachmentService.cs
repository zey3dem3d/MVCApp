using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.MVCApp.BLL.Common.Service.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        // Allowed Extensions
        public readonly List<string> _allowedExtensions = [".png", ".jpg", ".jpeg"];
        public const int _allowedSize = 2_097_152;
        public string? Upload(IFormFile file, string folderName)
        {
            // Validate For Extension
            var extension = Path.GetExtension(file.FileName);

            if (!_allowedExtensions.Contains(extension))
                return null;

            // Validate For Size
            if (file.Length > _allowedSize)
                return null;

            // Get Folder Path
            //var folderPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\{folderName}";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Set Unique File Name
            var fileName = $"{Guid.NewGuid()}{extension}";

            // Get File Path [Folder Path + File Name]
            var filePath = Path.Combine(folderPath, fileName); // File Location

            // Save File As Stream [Data Per Time]
            using var fileStream = new FileStream(filePath, FileMode.Create);

            // Copy File To The Stream
            file.CopyTo(fileStream);

            // Return File Name
            return fileName;

        }

        public bool Delete(string fileName, string folderName = "images")
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);
            var fullPath = Path.Combine(folderPath, fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }
    }
}
