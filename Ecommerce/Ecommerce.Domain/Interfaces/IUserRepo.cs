using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Interfaces
{
  public interface IUserRepo :
    IBaseRepo<User, UserFilterOptions>,
    IUpsert<User, UserFilterOptions>,
    IPartialUpdate<User, UserFilterOptions>

  {
    public Task<User?> GetUserByEmailAsync(string email);
  }
}
