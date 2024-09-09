using Ecommerce.Domain.Common;
using Ecommerce.Domain.Filters;

namespace Ecommerce.Domain.Interfaces
{
    public interface IUpsert<T, TFilter> : IBaseRepo<T, TFilter>
        where T : BaseEntity
        where TFilter : PaginationOptionsBase
    {
        Task<T> UpsertAsync(T entity, int? id = null);
    }
}