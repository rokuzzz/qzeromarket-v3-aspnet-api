using System.Data;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Services.ProductService.DTO
{
    public class CreateOrUpdateProductDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public required int CategoryId { get; set; }

        public List<IFormFile>? ProductImage { get; set; } = [];
        [SwaggerSchema(ReadOnly = true)]
        public List<string>? ProductImagePath { get; set; } = [];
    }

    public class CreateOrUpdateProductDtoValidator : AbstractValidator<CreateOrUpdateProductDto>
    {
        public CreateOrUpdateProductDtoValidator()
        {
            RuleFor(x => x.ProductImage).NotNull().NotEmpty();
            RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(3).MaximumLength(500);
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Stock).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}