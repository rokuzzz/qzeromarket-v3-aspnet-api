using Ecommerce.Domain.Models;
using Ecommerce.Services.ProductService.DTO;
using Ecommerce.Services.Extensions;
using Xunit;

namespace Ecommerce.Tests.Services
{
    public class ProductMappingExtensionsTest
    {

        private readonly int _id = 1;

        [Fact]

        public void ToProduct_CreateProductDto_ReturnsNewProduct()
        {
            var dto = new CreateOrUpdateProductDto
            {
                Title = "Best Product Ever",
                Description = "Buy me",
                Price = 20.0m,
                CategoryId = 2,
                Stock = 2
            };

            var result = dto.ToProduct(_id);

            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Best Product Ever", result.Title);
        }

    }
}
