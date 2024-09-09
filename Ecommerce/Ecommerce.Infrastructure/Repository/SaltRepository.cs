using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repository
{
    public class SaltRepository(EcommerceContext ecommerceContext) : ISaltRepo
    {
        private readonly EcommerceContext _ecommerceContext = ecommerceContext;
        public async Task<byte[]> GetSaltAsync(User user)
        {
            var result = await _ecommerceContext.SaltUsers
                .FirstOrDefaultAsync(x => x.UserId == user.Id);

            return result?.Salt ?? throw new Exception("Salt not found");
        }

        public async Task<bool> SetSaltAsync(User user, byte[] salt)
        {
            try
            {
                var currentUser = await _ecommerceContext.SaltUsers.FirstOrDefaultAsync(x => x.UserId == user.Id);
                if (currentUser != null)
                {
                    _ecommerceContext.SaltUsers.Add(new SaltUser { UserId = currentUser.UserId, Salt = salt, User = currentUser.User });
                }
                else
                {
                    _ecommerceContext.SaltUsers.Update(new SaltUser { UserId = user.Id, Salt = salt, User = user });
                }
                _ecommerceContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
