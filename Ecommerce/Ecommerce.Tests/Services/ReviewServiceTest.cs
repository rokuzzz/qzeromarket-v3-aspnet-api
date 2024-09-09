using Moq;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Services.ReviewService;
using Ecommerce.Services.ReviewService.DTO;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Common.Exceptions;
using Ecommerce.Domain.Filters;
using Ecommerce.Services.Common;
using Xunit;

namespace Ecommerce.Tests.Services
{
    public class ReviewServiceTest
    {
        private readonly Mock<IReviewRepo> _mockReviewRepo;
        private readonly ReviewService _reviewService;

        public ReviewServiceTest()
        {
            _mockReviewRepo = new Mock<IReviewRepo>();
            _reviewService = new ReviewService(_mockReviewRepo.Object);
        }

      

        [Fact]
        public async Task PartialUpdateAsync_WhenCalledWithPartialUpdateDto_ReturnsGetDto()
        {
            var review = new Review
            {
                Id = 2,
                Title = "Not Bad",
                Description = "Pretty decent",
                Rating = 4,
                ProductId = 1,
                UserId = 1
            };

            _mockReviewRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(review);

            _mockReviewRepo
                .Setup(repo => repo.PartialUpdateByIdAsync(It.IsAny<Review>(), It.IsAny<int>()))
                .ReturnsAsync((Review review, int id) => review);

            var dto = new PartialUpdateReviewDto
            {
                Rating = 3
            };

            var result = await _reviewService.PartialUpdateByIdAsync(dto, 2);

            Assert.NotNull(result);
            Assert.IsType<GetReviewDto>(result);
            Assert.Equal(3, result.Rating);

            _mockReviewRepo.Verify(repo => repo.GetByIdAsync(2), Times.Once);
            _mockReviewRepo.Verify(repo => repo.PartialUpdateByIdAsync(review, 2), Times.Once);
        }
    }
}
