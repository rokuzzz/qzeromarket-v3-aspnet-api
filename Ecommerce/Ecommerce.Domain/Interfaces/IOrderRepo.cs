using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Interfaces
{
    public interface IOrderRepo :
        IBaseRepo<Order, OrderFilterOptions>,
        ICreate<Order, OrderFilterOptions>
    { }
};
