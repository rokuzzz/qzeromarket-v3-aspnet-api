using Ecommerce.Domain.Models;
using Ecommerce.Services.Common.DTO;

namespace Ecommerce.Services.ReviewService.DTO
{
    
    public class GetReviewDto : IReadDto<Review>
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Rating { get; set; }
        public required int UserId { get; set; }
        public required int ProductId { get; set; }



        public void FromEntity(Review entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Description = entity.Description;
            Rating = entity.Rating;
            UserId = entity.UserId;
            ProductId = entity.ProductId;
        }
    }
}