using FluentValidation;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Services.CategoryService.DTO
{
    public class CreateOrUpdateCategoryDto
    {
        public required string Name { get; set; }
        public required IFormFile CategoryImage { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public string? CategoryImagePath { get; set; }
        public int? ParentCategoryId { get; set; }
    }

    public class CreateOrUpdateCategoryDtoValidator : AbstractValidator<CreateOrUpdateCategoryDto>
    {
        public CreateOrUpdateCategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.CategoryImage).NotNull();
        }
    }
}