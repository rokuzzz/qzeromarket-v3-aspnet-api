using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Interfaces
{
    public interface ICategoryRepo :
        IBaseRepo<Category, CategoryFilterOptions>,
        IUpsert<Category, CategoryFilterOptions>,
        IPartialUpdate<Category, CategoryFilterOptions>
    { }
}