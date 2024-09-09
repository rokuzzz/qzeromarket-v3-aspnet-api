using Ecommerce.Domain.Common.Exceptions;
using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repository
{
    public class ReviewRepository(EcommerceContext context) : IReviewRepo
    {
        private readonly EcommerceContext _context = context;

        public async Task<int> CountAsync(ReviewFilterOptions filteringOptions)
        {
            string query;
            object[] parameters;

            {
                query = "SELECT * FROM count_reviews({0})";
                parameters = [filteringOptions.ProductId];
            }

            var count = await _context.Database
                .SqlQueryRaw<int>(query, parameters)
                .ToListAsync();

            return count.FirstOrDefault();
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var query = "SELECT * FROM delete_review({0})";
            var result = await _context.Database
                .SqlQueryRaw<bool>(query, id)
                .ToListAsync();

            if (!result.FirstOrDefault())
            {
                throw new ReviewNotFoundException();
            }
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Review>> GetAllAsync(ReviewFilterOptions filteringOptions)
        {
            var page = filteringOptions.Page ?? 1;
            var perPage = filteringOptions.PerPage ?? 10;
            return await _context
                .GetReviews(filteringOptions.ProductId)
                .Where(r => r.ProductId == filteringOptions.ProductId)
                .OrderBy(r => r.Id)
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .ToListAsync();
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            var result = await _context.GetReviewById(id).FirstOrDefaultAsync();
            return result ?? throw new ReviewNotFoundException();
        }

        public async Task<Review> PartialUpdateByIdAsync(Review entity, int id)
        {
            return await _context.PatchReview(id, entity.ProductId, entity.UserId, entity.Rating, entity.Title, entity.Description).FirstAsync();
        }

        public async Task<Review> UpsertAsync(Review entity, int? id)
        {
            return await _context.UpsertReview(entity.ProductId, entity.UserId, entity.Rating, entity.Title, entity.Description, id).FirstAsync();
        }
    }
}