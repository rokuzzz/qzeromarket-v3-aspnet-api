using Ecommerce.Domain.Models;
using Ecommerce.Services.ReviewService.DTO;
using Ecommerce.Services.Extensions;
using Xunit;

namespace Ecommerce.Tests.Services
{
    public class ReviewMappingExtensionsTest
    {
        
        private readonly int _id = 1;
        [Fact]
        
        public void ToReview_CreateReviewDto_ReturnsNewReview()
        {
            var dto = new CreateOrUpdateReviewDto
            {
                Title = "Awesome!",
                Description = "Great product.",
                Rating = 5,
                ProductId = 1,
                UserId = 1
            };

            var result = dto.ToReview(_id);

            Assert.NotNull(result);
            Assert.IsType<Review>(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Awesome!", result.Title);
            Assert.Equal(5, result.Rating);
        }

        [Fact]
        public void ToReview_PartialUpdateReviewDto_ReturnsUpdatedReview()
        {
            var entity = new Review
            {
                Id = 0,
                Title = "Okay",
                Description = "Not bad.",
                Rating = 3,
                ProductId = 1,
                UserId = 1
            };

            var dto = new PartialUpdateReviewDto
            {
                Rating = 4
            };

            var result = dto.ToReview(entity);

            Assert.NotNull(result);
            Assert.IsType<Review>(result);
            Assert.Equal(4, result.Rating);
            Assert.Same(result, entity);
        }

        [Fact]
        public void ToGetReviewDto_Review_ReturnsGetReviewDto()
        {
            var entity = new Review
            {
                Id = 0,
                Title = "Good",
                Description = "Quite good.",
                Rating = 4,
                ProductId = 1,
                UserId = 1
            };

            var result = entity.ToGetReviewDto();

            Assert.NotNull(result);
            Assert.IsType<GetReviewDto>(result);
            Assert.Equal("Good", result.Title);
            Assert.Equal(4, result.Rating);
        }
    }
}
