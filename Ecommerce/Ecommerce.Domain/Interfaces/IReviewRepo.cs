using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Interfaces
{
    public interface IReviewRepo :
        IBaseRepo<Review, ReviewFilterOptions>,
        IUpsert<Review, ReviewFilterOptions>,
        IPartialUpdate<Review, ReviewFilterOptions>
    { }
}