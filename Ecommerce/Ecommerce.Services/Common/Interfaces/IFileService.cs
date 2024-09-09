using Microsoft.AspNetCore.Http;

namespace Ecommerce.Services.Common.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string prefix);
    }
}