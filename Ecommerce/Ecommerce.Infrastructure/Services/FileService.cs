using Ecommerce.Services.Common.Interfaces;

namespace Ecommerce.Infrastructure.Services
{
    public class FileService(IWebHostEnvironment environment) : IFileService
    {
        private readonly IWebHostEnvironment _environment = environment;

        public async Task<string> SaveFileAsync(IFormFile file, string directory)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid file.");
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";

            var uploadsFolderPath = Path.Combine(_environment.WebRootPath, directory);

            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(directory, uniqueFileName);
        }
    }
}