using FluentValidation;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Services.ReviewService.DTO
{
    public class CreateOrUpdateReviewDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Rating { get; set; }
        public required int UserId { get; set; }
        public required int ProductId { get; set; }

    }

    public class CreateOrUpdateReviewDtoValidator : AbstractValidator<CreateOrUpdateReviewDto>
    {
        public CreateOrUpdateReviewDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(3).MaximumLength(500);
            RuleFor(x => x.Rating).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(5);
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
        }
    }
}