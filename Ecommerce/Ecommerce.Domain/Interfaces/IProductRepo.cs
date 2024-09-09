using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Interfaces
{
    public interface IProductRepo :
        IBaseRepo<Product, ProductFilterOptions>,
        IUpsert<Product, ProductFilterOptions>,
        IPartialUpdate<Product, ProductFilterOptions>
    { }
}