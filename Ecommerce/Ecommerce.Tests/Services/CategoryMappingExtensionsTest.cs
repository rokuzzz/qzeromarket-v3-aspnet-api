using Ecommerce.Domain.Models;
using Ecommerce.Services.CategoryService.DTO;
using Ecommerce.Services.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Ecommerce.Tests.Services
{
    public class CategoryMappingExtensionsTest
    {
        private readonly string _name = "Category";
        private readonly string _image = "Image";
        private readonly int _id = 1;
        private readonly IFormFile _formFile = new Mock<IFormFile>().Object;

        [Fact]
        public void ToCategory_CreateOrUpdateCategoryDto_ReturnsNewCategory()
        {
            var dto = new CreateOrUpdateCategoryDto
            {
                Name = _name,
                CategoryImagePath = _image,
                CategoryImage = _formFile,
            };

            var result = dto.ToCategory(_id);

            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(_id, result.Id);
            Assert.Equal(_name, result.Name);
            Assert.Equal(_image, result.CategoryImage);
            Assert.Null(result.ParentCategoryId);
        }

        [Fact]
        public void ToCategory_PartialUpdateCategoryDto_ReturnsUpdatedCategory()
        {
            var entity = new Category
            {
                Id = _id,
                Name = _name,
                CategoryImage = _image,
                ParentCategoryId = 1,
                SubCategories = [],
                Products = []
            };

            var dto = new PartialUpdateCategoryDto
            {
                Name = "Updated",
                CategoryImagePath = "Updated",
                ParentCategoryId = 3,
            };

            var result = dto.ToCategory(entity);

            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(_id, result.Id);
            Assert.Equal("Updated", result.Name);
            Assert.Equal("Updated", result.CategoryImage);
            Assert.Equal(3, result.ParentCategoryId);
            Assert.Same(result, entity);
        }

        [Fact]
        public void ToGetCategoryDto_Category_ReturnsGetCategoryDto()
        {
            var entity = new Category
            {
                Id = _id,
                Name = _name,
                CategoryImage = _image,
                ParentCategoryId = 3,
                SubCategories = [],
                Products = []

            };

            var result = entity.ToGetCategoryDto();

            Assert.NotNull(result);
            Assert.IsType<GetCategoryDto>(result);
            Assert.Equal(_id, result.Id);
            Assert.Equal(_name, result.Name);
            Assert.Equal(_image, result.CategoryImage);
            Assert.Equal(3, result.ParentCategoryId);
        }
    }
}
