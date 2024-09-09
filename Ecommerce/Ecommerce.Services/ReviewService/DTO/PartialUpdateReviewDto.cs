using FluentValidation;

namespace Ecommerce.Services.ReviewService.DTO
{
    public class PartialUpdateReviewDto()
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Rating { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
    }

    public class PartialUpdateReviewDtoValidator : FluentValidation.AbstractValidator<PartialUpdateReviewDto>
    {
        public PartialUpdateReviewDtoValidator()
        {
            RuleFor(x => x.Title).MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Description).MinimumLength(3).MaximumLength(500);
            RuleFor(x => x.Rating).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5);
        }
    }
}