using Ecommerce.Domain.Models;
using Ecommerce.Services.Common.DTO;

namespace Ecommerce.Services.CategoryService.DTO
{
    public class GetCategoryDto : IReadDto<Category>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string CategoryImage { get; set; }
        public int? ParentCategoryId { get; set; }

        public void FromEntity(Category entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            CategoryImage = entity.CategoryImage;
            ParentCategoryId = entity.ParentCategoryId;
        }
    }
}