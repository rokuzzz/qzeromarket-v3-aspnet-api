using Ecommerce.Domain.Common.Exceptions;
using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repository
{
    public class CategoryRepository(EcommerceContext context) : ICategoryRepo
    {
        private readonly EcommerceContext _context = context;

        public async Task<int> CountAsync(CategoryFilterOptions filteringOptions)
        {
            string query;
            object[] parameters;

            if (filteringOptions.ParentCategoryId.HasValue)
            {
                query = "SELECT * FROM count_categories({0})";
                parameters = [filteringOptions.ParentCategoryId.Value];
            }
            else
            {
                query = "SELECT * FROM count_categories()";
                parameters = [];
            }

            var count = await _context.Database
                .SqlQueryRaw<int>(query, parameters)
                .ToListAsync();

            return count.FirstOrDefault();
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var query = "SELECT * FROM delete_category({0})";
            var result = await _context.Database
                .SqlQueryRaw<bool>(query, id)
                .ToListAsync();

            if (!result.FirstOrDefault())
            {
                throw new CategoryNotFoundException();
            }

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CategoryFilterOptions filteringOptions)
        {
            return await _context.GetCategories(filteringOptions.Page, filteringOptions.PerPage, filteringOptions.ParentCategoryId).OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var result = await _context.GetCategoryById(id).FirstOrDefaultAsync();
            return result ?? throw new CategoryNotFoundException();
        }

        public async Task<Category> PartialUpdateByIdAsync(Category entity, int id)
        {
            return await _context.PatchCategory(id, entity.Name, entity.CategoryImage, entity.ParentCategoryId).FirstAsync();
        }

        public async Task<Category> UpsertAsync(Category entity, int? id)
        {
            return await _context.UpsertCategory(entity.Name, entity.CategoryImage, entity.ParentCategoryId, id).FirstAsync();
        }
    }
}