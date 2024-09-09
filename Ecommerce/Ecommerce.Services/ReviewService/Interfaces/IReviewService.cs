using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;
using Ecommerce.Services.ReviewService.DTO;
using Ecommerce.Services.Common.Interfaces;

namespace Ecommerce.Services.ReviewService.Interfaces
{
    public interface IReviewService : IBaseService<Review, ReviewFilterOptions, GetReviewDto>
    {
        Task<GetReviewDto> UpsertAsync(CreateOrUpdateReviewDto dto, int? id = null);
        Task<GetReviewDto> PartialUpdateByIdAsync(PartialUpdateReviewDto dto, int id);
    }
}