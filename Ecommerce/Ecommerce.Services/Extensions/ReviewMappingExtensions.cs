using Ecommerce.Domain.Models;
using Ecommerce.Services.ReviewService.DTO;

namespace Ecommerce.Services.Extensions
{
    public static class ReviewMappingExtensions
    {
        public static Review ToReview(this CreateOrUpdateReviewDto dto, int? id)
        {
            return new Review
            {
                Id = id ?? 0,
                Title = dto.Title,
                Description = dto.Description,
                Rating = dto.Rating,
                UserId = dto.UserId,
                ProductId = dto.ProductId,
                User = null!,
                Product = null!,
            };
        }

        public static Review ToReview(this PartialUpdateReviewDto dto, Review entity)
        {
            entity.Title = dto.Title ?? entity.Title;
            entity.Description = dto.Description ?? entity.Description;
            entity.Rating = dto.Rating ?? entity.Rating;
            entity.UserId = dto.UserId ?? entity.UserId;
            entity.ProductId = dto.ProductId ?? entity.ProductId;

            return entity;
        }

        public static GetReviewDto ToGetReviewDto(this Review entity)
        {
            return new GetReviewDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Rating = entity.Rating,
                UserId = entity.UserId,
                ProductId = entity.ProductId,
            };
        }
    }
}