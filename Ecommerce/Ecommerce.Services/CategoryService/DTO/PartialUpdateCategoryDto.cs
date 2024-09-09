using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Services.CategoryService.DTO
{
    public class PartialUpdateCategoryDto()
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public IFormFile? CategoryImage { get; set; }
        public int? ParentCategoryId { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public string? CategoryImagePath { get; set; }
    }

    public class PartialUpdateCategoryDtoValidator : AbstractValidator<PartialUpdateCategoryDto>
    {
        public PartialUpdateCategoryDtoValidator()
        {
            RuleFor(x => x.Name).MinimumLength(3).MaximumLength(50);
        }
    }

}