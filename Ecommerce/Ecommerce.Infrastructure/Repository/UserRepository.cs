using Ecommerce.Domain.Common.Exceptions;
using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repository
{
	public class UserRepository(EcommerceContext context) : IUserRepo
	{
		private readonly EcommerceContext _context = context;

		public async Task<int> CountAsync(UserFilterOptions filteringOptions)
		{
			string query;
			object[] parameters;

			if (filteringOptions != null && filteringOptions.Role.HasValue)
			{
				query = "SELECT * FROM count_users({0})";
				parameters = [filteringOptions.Role.Value.ToString().ToLower()];
			}
			else
			{
				query = "SELECT * FROM count_users()";
				parameters = [];
			}

			var count = await _context.Database
				.SqlQueryRaw<int>(query, parameters)
				.ToListAsync();

			return count.FirstOrDefault();
		}

		public async Task<bool> DeleteByIdAsync(int id)
		{
			var query = "SELECT * FROM delete_user_by_id({0})";
			var result = await _context.Database
				.SqlQueryRaw<bool>(query, id)
				.ToListAsync();

			if (!result.FirstOrDefault())
			{
				throw new UserNotFoundException();
			}

			return result.FirstOrDefault();
		}

		public async Task<IEnumerable<User>> GetAllAsync(UserFilterOptions? filteringOptions)
		{
			int perPage = filteringOptions?.PerPage ?? 10;
			int page = filteringOptions?.Page ?? 1;

			string query;
			object[] parameters;

			if (filteringOptions != null && filteringOptions?.Role.HasValue == true)
			{
				query = "SELECT * FROM users WHERE role = {0}";
				parameters = [filteringOptions.Role.Value.ToString().ToLower()];
			}
			else
			{
				query = "SELECT * FROM users";
				parameters = [];
			}

			var users = await _context.Users
				.FromSqlRaw(query, parameters)
				.OrderBy(u => u.Id)
				.Skip(perPage * (page - 1))
				.Take(perPage)
				.ToListAsync();

			return users;
		}

		public async Task<User> GetByIdAsync(int id)
		{
			var result = await _context.GetUserById(id).FirstOrDefaultAsync();
			return result ?? throw new UserNotFoundException();
		}

		public Task<User?> GetUserByEmailAsync(string email)
		{
			return _context.Users.FirstOrDefaultAsync(u => u.Email == email);
		}

		public async Task<User> PartialUpdateByIdAsync(User entity, int id)
		{
			return await _context.PatchUser(id, entity.Email, entity.FirstName, entity.LastName, entity.Password, entity.Avatar).FirstAsync();
		}

		public async Task<User> UpsertAsync(User entity, int? id = null)
		{
			return await _context.UpdateUser(id ?? entity.Id, entity.Email, entity.FirstName, entity.LastName, entity.Password, entity.Role.ToString().ToLower(), entity.Avatar).FirstAsync();
		}
	}
}