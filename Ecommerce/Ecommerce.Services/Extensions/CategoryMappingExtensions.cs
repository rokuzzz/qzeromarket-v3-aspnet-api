using Ecommerce.Domain.Models;
using Ecommerce.Services.CategoryService.DTO;

namespace Ecommerce.Services.Extensions
{
    public static class CategoryMappingExtensions
    {
        public static Category ToCategory(this CreateOrUpdateCategoryDto dto, int? id)
        {
            return new Category
            {
                Id = id ?? 0,
                Name = dto.Name,
                CategoryImage = dto.CategoryImagePath ?? dto.CategoryImage.FileName,
                ParentCategoryId = dto.ParentCategoryId,
                SubCategories = [],
                Products = []
            };
        }

        public static Category ToCategory(this PartialUpdateCategoryDto dto, Category entity)
        {
            entity.Name = dto.Name ?? entity.Name;
            entity.CategoryImage = dto.CategoryImagePath ?? entity.CategoryImage;
            entity.ParentCategoryId = dto.ParentCategoryId ?? entity.ParentCategoryId;

            return entity;
        }

        public static GetCategoryDto ToGetCategoryDto(this Category entity)
        {
            return new GetCategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CategoryImage = entity.CategoryImage,
                ParentCategoryId = entity.ParentCategoryId
            };
        }
    }
}