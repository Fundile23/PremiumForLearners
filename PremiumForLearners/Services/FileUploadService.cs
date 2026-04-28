using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace PremiumForLearners.Services
{
    public class FileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadFolder;

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _uploadFolder = Path.Combine(_environment.WebRootPath, "uploads");

            // Create upload folder if it doesn't exist
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file, string subFolder = "")
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            // Validate file type (PDF, JPG, PNG)
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Only PDF, JPG, and PNG files are allowed");

            // Validate file size (max 5MB)
            if (file.Length > 5 * 1024 * 1024)
                throw new Exception("File size cannot exceed 5MB");

            // Create subfolder if specified
            var targetFolder = _uploadFolder;
            if (!string.IsNullOrEmpty(subFolder))
            {
                targetFolder = Path.Combine(_uploadFolder, subFolder);
                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);
            }

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
            var filePath = Path.Combine(targetFolder, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path
            var relativePath = Path.Combine("uploads", subFolder, fileName).Replace("\\", "/");
            return $"/{relativePath}";
        }

        public bool DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            try
            {
                var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
            }
            catch { }

            return false;
        }
    }
}