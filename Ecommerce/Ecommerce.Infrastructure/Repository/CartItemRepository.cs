using Ecommerce.Domain.Common.Exceptions;
using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Ecommerce.Infrastructure.Repository
{
    public class CartItemRepository(EcommerceContext context) : ICartItemRepo
    {
        private readonly EcommerceContext _context = context;

        public async Task<int> CountAsync(CartItemFilterOptions filteringOptions)
        {
            string query = "SELECT * FROM count_cart_items({0})";
            object[] parameters = [filteringOptions.UserId];

            var count = await _context.Database
                .SqlQueryRaw<int>(query, parameters)
                .ToListAsync();

            return count.FirstOrDefault();
        }

        public async Task<CartItem> CreateAsync(CartItem entity)
        {
            try
            {
                return await _context.CreateCartItem(entity.UserId, entity.ProductId, entity.Quantity).FirstAsync();
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "P0001")
                {
                    switch (ex.MessageText)
                    {
                        case "Invalid user id":
                            throw new UserNotFoundException();
                        case "Invalid product id":
                            throw new ProductNotFoundException();
                        default:
                            throw;
                    }
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var query = "SELECT * FROM delete_cart_item({0})";
            var result = await _context.Database
                .SqlQueryRaw<bool>(query, id)
                .ToListAsync();

            if (!result.FirstOrDefault())
            {
                throw new CartItemNotFoundExcepiton();
            }

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync(CartItemFilterOptions filteringOptions)
        {
            return await _context.GetCartItemsForUser(filteringOptions.UserId).OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<CartItem> GetByIdAsync(int id)
        {
            var result = await _context.GetCartItemById(id).FirstOrDefaultAsync();
            return result ?? throw new CartItemNotFoundExcepiton();
        }

        public async Task<CartItem> PartialUpdateByIdAsync(CartItem entity, int id)
        {
            return await _context.PatchCartItem(id, entity.Quantity).FirstAsync();
        }
    }
}