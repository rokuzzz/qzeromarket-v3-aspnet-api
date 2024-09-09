using Ecommerce.Domain.Common;
using Ecommerce.Domain.Filters;

namespace Ecommerce.Domain.Interfaces
{
    public interface ICreate<T, TFilter> : IBaseRepo<T, TFilter>
        where T : BaseEntity
        where TFilter : PaginationOptionsBase
    {
        Task<T> CreateAsync(T entity);
    }
}