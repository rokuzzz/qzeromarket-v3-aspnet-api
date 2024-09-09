using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Services.ProductService.DTO
{
    public class PartialUpdateProductDto()
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public int? CategoryId { get; set; }
    }

    public class PartialUpdateProductDtoValidator : AbstractValidator<PartialUpdateProductDto>
    {
        public PartialUpdateProductDtoValidator()
        {
            RuleFor(x => x.Title).MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Description).MinimumLength(3).MaximumLength(500);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CategoryId).GreaterThanOrEqualTo(1);
        }
    }
}