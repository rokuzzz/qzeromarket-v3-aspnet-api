using Moq;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Services.ProductService;
using Ecommerce.Services.ProductService.DTO;
using Ecommerce.Domain.Models;
using Xunit;
using System.Threading.Tasks;

namespace Ecommerce.Tests.Services
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepo> _mockProductRepo;
        private readonly ProductService _productService;

        public ProductServiceTest()
        {
            _mockProductRepo = new Mock<IProductRepo>();
            _productService = new ProductService(_mockProductRepo.Object);
        }

        [Fact]
        public async Task CreateAsync_WhenCalled_ReturnsGetDto()
        {
            _mockProductRepo
                .Setup(repo => repo.UpsertAsync(It.IsAny<Product>(), It.IsAny<int?>()))
                .ReturnsAsync((Product product, int? id) => product); // Corrected callback parameter count

            var dto = new CreateOrUpdateProductDto
            {
                Title = "New Product",
                Description = "New Product Description",
                Price = 99,
                CategoryId = 1,
                Stock = 12,
            };

            var result = await _productService.UpsertAsync(dto);

            Assert.NotNull(result);
            Assert.IsType<GetProductDto>(result);
            Assert.Equal("New Product", result.Title);
            Assert.Equal(99, result.Price);

            _mockProductRepo.Verify(repo => repo.UpsertAsync(It.IsAny<Product>(), It.IsAny<int?>()), Times.Once);
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenCalledWithExistingId_ReturnsTrue()
        {
            _mockProductRepo
                .Setup(repo => repo.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var result = await _productService.DeleteByIdAsync(1);

            Assert.True(result);

            _mockProductRepo.Verify(repo => repo.DeleteByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCalledWithExistingId_ReturnsGetDtoAsync()
        {
            var product = new Product
            {
                Id = 1,
                Title = "Product 1",
                Description = "Product Description",
                Price = 19,
                CategoryId = 1
            };

            _mockProductRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);

            var result = await _productService.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.IsType<GetProductDto>(result);
            Assert.Equal(19, result.Price);
            Assert.Equal("Product 1", result.Title);

            _mockProductRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }
    }
}
