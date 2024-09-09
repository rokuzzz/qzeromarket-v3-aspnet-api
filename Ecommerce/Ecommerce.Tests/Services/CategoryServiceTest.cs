using Moq;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Services.CategoryService;
using Ecommerce.Services.CategoryService.DTO;
using Ecommerce.Domain.Models;
using Microsoft.AspNetCore.Http;
using Ecommerce.Domain.Common.Exceptions;
using Ecommerce.Domain.Filters;
using Ecommerce.Services.Common;

namespace Ecommerce.Tests.Services
{
    public class CategoryServiceTest
    {
        private readonly Mock<ICategoryRepo> _mockCategoryRepo;
        private readonly CategoryService _categoryService;

        public CategoryServiceTest()
        {
            _mockCategoryRepo = new Mock<ICategoryRepo>();
            _categoryService = new CategoryService(_mockCategoryRepo.Object);
        }

        [Fact]
        public async Task UpsertAsync_WhenCalledWithCreateDto_ReturnsGetDto()
        {
            _mockCategoryRepo
                .Setup(repo => repo.UpsertAsync(It.IsAny<Category>(), It.IsAny<int?>()))
                .ReturnsAsync((Category category, int? id) => category);

            var dto = new CreateOrUpdateCategoryDto
            {
                Name = "Category",
                CategoryImagePath = "Image",
                CategoryImage = new Mock<IFormFile>().Object
            };

            var result = await _categoryService.UpsertAsync(dto);

            Assert.NotNull(result);
            Assert.IsType<GetCategoryDto>(result);
            Assert.Equal("Category", result.Name);
            Assert.Equal("Image", result.CategoryImage);

            _mockCategoryRepo.Verify(repo => repo.UpsertAsync(It.IsAny<Category>(), It.IsAny<int?>()), Times.Once);
        }

        [Fact]
        public async Task UpsertAsync_WhenCalledWithUpdateDto_ReturnsGetDto()
        {
            _mockCategoryRepo
                .Setup(repo => repo.UpsertAsync(It.IsAny<Category>(), It.IsAny<int>()))
                .ReturnsAsync((Category category, int id) => category);

            var dto = new CreateOrUpdateCategoryDto
            {
                Name = "Category",
                CategoryImagePath = "Image",
                CategoryImage = new Mock<IFormFile>().Object
            };

            var result = await _categoryService.UpsertAsync(dto, 1);

            Assert.NotNull(result);
            Assert.IsType<GetCategoryDto>(result);
            Assert.Equal("Category", result.Name);
            Assert.Equal("Image", result.CategoryImage);

            _mockCategoryRepo.Verify(repo => repo.UpsertAsync(It.IsAny<Category>(), 1), Times.Once);
        }

        [Fact]
        public async Task UpsertAsync_WhenCalledWithUpdateDtoWithWrongId_ReturnsGetDtoWithNewId()
        {
            _mockCategoryRepo
                .Setup(repo => repo.UpsertAsync(It.IsAny<Category>(), It.IsAny<int>()))
                .ReturnsAsync((Category category, int id) =>
                {
                    category.Id = ++id;
                    return category;
                });

            var dto = new CreateOrUpdateCategoryDto
            {
                Name = "Category",
                CategoryImagePath = "Image",
                CategoryImage = new Mock<IFormFile>().Object
            };

            var result = await _categoryService.UpsertAsync(dto, 1);

            Assert.NotNull(result);
            Assert.IsType<GetCategoryDto>(result);
            Assert.Equal("Category", result.Name);
            Assert.Equal("Image", result.CategoryImage);
            Assert.Equal(2, result.Id);

            _mockCategoryRepo.Verify(repo => repo.UpsertAsync(It.IsAny<Category>(), 1), Times.Once);
        }

        [Fact]
        public async Task PartialUpdateByIdAsync_WhenCalledWithPartialUpdateDto_ReturnsGetDto()
        {
            var category = new Category
            {
                Id = 2,
                Name = "Category1",
                CategoryImage = "Image1",
                Products = [],
                SubCategories = []
            };

            _mockCategoryRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(category);

            _mockCategoryRepo
                .Setup(repo => repo.PartialUpdateByIdAsync(It.IsAny<Category>(), It.IsAny<int>()))
                .ReturnsAsync((Category category, int id) => category);

            var dto = new PartialUpdateCategoryDto
            {
                Name = "Category2",
                CategoryImagePath = "Image2",
                ParentCategoryId = 1,
            };

            var result = await _categoryService.PartialUpdateByIdAsync(dto, 1);

            Assert.NotNull(result);
            Assert.IsType<GetCategoryDto>(result);
            Assert.Equal("Category2", result.Name);
            Assert.Equal("Image2", result.CategoryImage);
            Assert.Equal(1, result.ParentCategoryId);

            _mockCategoryRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _mockCategoryRepo.Verify(repo => repo.PartialUpdateByIdAsync(category, 1), Times.Once);
        }

        [Fact]
        public async Task PartialUpdateByIdAsync_WhenCalledWithPartialUpdateDtoAndCategoryNotFound_ThrowsException()
        {
            _mockCategoryRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new CategoryNotFoundException());

            var dto = new PartialUpdateCategoryDto
            {
                Name = "Category2",
                CategoryImagePath = "Image2",
                ParentCategoryId = 1,
            };

            await Assert.ThrowsAsync<CategoryNotFoundException>(() => _categoryService.PartialUpdateByIdAsync(dto, 1));

            _mockCategoryRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _mockCategoryRepo.Verify(repo => repo.PartialUpdateByIdAsync(It.IsAny<Category>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenCalledWithExistingId_ReturnsTrue()
        {
            _mockCategoryRepo
                .Setup(repo => repo.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var result = await _categoryService.DeleteByIdAsync(1);

            Assert.True(result);

            _mockCategoryRepo.Verify(repo => repo.DeleteByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenCalledWithWrongId_ReturnsFalse()
        {
            _mockCategoryRepo
                .Setup(repo => repo.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var result = await _categoryService.DeleteByIdAsync(1);

            Assert.False(result);

            _mockCategoryRepo.Verify(repo => repo.DeleteByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCalledWithExistingId_ReturnsGetDtoAsync()
        {
            var category = new Category
            {
                Id = 1,
                Name = "Category",
                CategoryImage = "Image",
                Products = [],
                SubCategories = []
            };

            _mockCategoryRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(category);

            var result = await _categoryService.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.IsType<GetCategoryDto>(result);
            Assert.Equal("Category", result.Name);
            Assert.Equal("Image", result.CategoryImage);

            _mockCategoryRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCalledWithWrongId_ThrowsException()
        {
            _mockCategoryRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new CategoryNotFoundException());

            await Assert.ThrowsAsync<CategoryNotFoundException>(() => _categoryService.GetByIdAsync(1));

            _mockCategoryRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenCalled_ReturnsPaginatedResult()
        {
            var category = new Category
            {
                Id = 1,
                Name = "Category",
                CategoryImage = "Image",
                Products = [],
                SubCategories = []
            };

            _mockCategoryRepo
                .Setup(repo => repo.GetAllAsync(It.IsAny<CategoryFilterOptions>()))
                .ReturnsAsync([category]);

            _mockCategoryRepo
                .Setup(repo => repo.CountAsync(It.IsAny<CategoryFilterOptions>()))
                .ReturnsAsync(1);

            var filteringOptions = new CategoryFilterOptions
            {
                Page = 1,
                PerPage = 10
            };

            var result = await _categoryService.GetAllAsync(filteringOptions);

            Assert.NotNull(result);
            Assert.IsType<PaginatedResult<Category, GetCategoryDto>>(result);
            Assert.Equal(10, result.ItemsPerPage);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(1, result.TotalItems);
            Assert.Single(result.Items);
            Assert.Equal("Category", result.Items.First().Name);
            Assert.Equal("Image", result.Items.First().CategoryImage);

            _mockCategoryRepo.Verify(repo => repo.GetAllAsync(filteringOptions), Times.Once);
        }
    }
}