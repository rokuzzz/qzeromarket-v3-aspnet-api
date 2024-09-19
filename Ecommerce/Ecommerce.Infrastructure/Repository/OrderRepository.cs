using Ecommerce.Domain.Common.Exceptions;
using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Ecommerce.Infrastructure.Repository
{
    public class OrderRepository(EcommerceContext context) : IOrderRepo
    {
        private readonly EcommerceContext _context = context;
        
        public async Task<int> CountAsync(OrderFilterOptions filteringOptions)
        {
            IQueryable<Order> query = _context.Orders;

            if (filteringOptions.UserId.HasValue)
            {
                query = query.Where(o => o.UserId == filteringOptions.UserId.Value);
            }

            return await query.CountAsync();
        }


        public async Task<Order> CreateAsync(Order entity)
        {
            try
            {
                var newOrder = await _context.CreateOrderFromCart(entity.UserId).FirstOrDefaultAsync() ?? throw new OrderNotFoundException();
                return await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == newOrder.Id) ?? throw new OrderNotFoundException();
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "P0002")
                {
                    throw new InsufficientStockException();
                }
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            string query = "SELECT * FROM delete_order({0})";

            var result = await _context.Database
                .SqlQueryRaw<bool>(query, id)
                .ToListAsync();

            if (!result.FirstOrDefault())
            {
                throw new OrderNotFoundException();
            }

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Order>> GetAllAsync(OrderFilterOptions filteringOptions)
        {
            int perPage = filteringOptions.PerPage ?? 10;
            int page = filteringOptions.Page ?? 1;

            IQueryable<Order> query = _context.Orders
                .Include(o => o.OrderItems);

            if (filteringOptions.UserId.HasValue)
            {
                query = query.Where(o => o.UserId == filteringOptions.UserId.Value);
            }

            var result = await query
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .ToListAsync();

            return result;
        }


        public async Task<Order> GetByIdAsync(int id)
        {
            var result = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
            return result ?? throw new OrderNotFoundException();
        }
    }
}