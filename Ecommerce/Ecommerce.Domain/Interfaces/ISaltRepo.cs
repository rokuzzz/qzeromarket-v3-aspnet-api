using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Interfaces
{
  public interface ISaltRepo
  {
    Task<byte[]> GetSaltAsync(User user);
    Task<bool> SetSaltAsync(User user, byte[] salt);
  }
}