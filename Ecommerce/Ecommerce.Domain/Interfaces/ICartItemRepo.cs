using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Interfaces
{
    public interface ICartItemRepo :
        IBaseRepo<CartItem, CartItemFilterOptions>,
        IPartialUpdate<CartItem, CartItemFilterOptions>,
        ICreate<CartItem, CartItemFilterOptions>
    { }
};
