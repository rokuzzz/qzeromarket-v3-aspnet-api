using Ecommerce.Domain.Common.Exceptions;
using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repository
{
    public class ProductRepository(EcommerceContext context) : IProductRepo
    {
        private readonly EcommerceContext _context = context;

        public async Task<int> CountAsync(ProductFilterOptions filteringOptions)
        {
            string query = "SELECT * FROM count_products()";
            var count = await _context.Database
                .SqlQueryRaw<int>(query)
                .ToListAsync();

            var totalCount = count.FirstOrDefault();

            if (filteringOptions?.CategoryId.HasValue == true)
            {
                // If CategoryId filter is applied, we need to count only the products in that category
                var productsInCategory = await _context.Products
                    .Where(p => p.CategoryId == filteringOptions.CategoryId.Value)
                    .CountAsync();

                return productsInCategory;
            }

            return totalCount;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var query = "SELECT * FROM delete_product({0})";
            var result = await _context.Database
                .SqlQueryRaw<bool>(query, id)
                .ToListAsync();

            if (!result.FirstOrDefault())
            {
                throw new ProductNotFoundException();
            }

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Product>> GetAllAsync(ProductFilterOptions filteringOptions)
        {
            int perPage = filteringOptions?.PerPage ?? 10;
            int page = filteringOptions?.Page ?? 1;

            IQueryable<Product> query = _context.Products.Include(p => p.ProductImages);

            if (filteringOptions?.CategoryId.HasValue == true)
            {
                query = query.Where(p => p.CategoryId == filteringOptions.CategoryId.Value);
            }

            var products = await query
                .OrderBy(p => p.Id)
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToListAsync();

            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var result = await _context.GetProductById(id).Include(p => p.ProductImages).FirstOrDefaultAsync();
            return result ?? throw new ProductNotFoundException();
        }

        public async Task<Product> PartialUpdateByIdAsync(Product entity, int id)
        {
            return await _context.PatchProduct(id, entity.Title, entity.Description, entity.Price, entity.Stock, entity.CategoryId).FirstAsync();
        }

        public async Task<Product> UpsertAsync(Product entity, int? id)
        {
            var Result = await _context.UpsertProduct(entity.Title, entity.Description, entity.Price, entity.Stock, entity.CategoryId, id).FirstAsync();
            var ProductImages = new List<ProductImage>();
            foreach (var ProductImage in entity.ProductImages)
            {
                ProductImage.ProductId = Result.Id;
                ProductImages.Add(ProductImage);
            }
            _context.ProductImages.AddRange(ProductImages);
            await _context.SaveChangesAsync();
            return await _context.Products.Where(p => p.Id == Result.Id).Include(p => p.ProductImages).FirstOrDefaultAsync() ?? throw new ProductNotFoundException();
        }
    }
}