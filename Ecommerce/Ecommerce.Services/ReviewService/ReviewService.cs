using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Services.ReviewService.DTO;
using Ecommerce.Services.ReviewService.Interfaces;
using Ecommerce.Services.Common;
using Ecommerce.Services.Extensions;

namespace Ecommerce.Services.ReviewService
{
    public class ReviewService(IReviewRepo ReviewRepo) : BaseService<Review, ReviewFilterOptions, GetReviewDto>(ReviewRepo), IReviewService
    {
        private readonly IReviewRepo _repo = ReviewRepo;

        public async Task<GetReviewDto> PartialUpdateByIdAsync(PartialUpdateReviewDto dto, int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            var updatedEntity = dto.ToReview(entity);
            var result = await _repo.PartialUpdateByIdAsync(updatedEntity, id);
            return result.ToGetReviewDto();
        }

        public async Task<GetReviewDto> UpsertAsync(CreateOrUpdateReviewDto dto, int? id = null)
        {
            var result = await _repo.UpsertAsync(dto.ToReview(id), id);
            return result.ToGetReviewDto();
        }
    }
}