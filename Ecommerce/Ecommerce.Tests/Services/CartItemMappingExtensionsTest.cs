using Ecommerce.Domain.Models;
using Ecommerce.Services.CartItemService.DTO;
using Ecommerce.Services.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Ecommerce.Tests.Services
{
    public class CartItemMappingExtensionsTest

    {
        private readonly IFormFile _formFile = new Mock<IFormFile>().Object;

        [Fact]
        public void ToCartItem_CreateCartItemDto_ReturnsNewCartItem()
        {
            var dto = new CreateCartItemDto
            {
                ProductId = 1,
                Quantity = 2,
                UserId = 1
            };

            var result = dto.ToCartItem();

            Assert.NotNull(result);
            Assert.IsType<CartItem>(result);
            Assert.Equal(0, result.Id);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(2, result.Quantity);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public void ToCartItem_PartialUpdateCartItemDto_ReturnsUpdatedCartItem()
        {
            var entity = new CartItem
            {
                Id = 0,
                ProductId = 1,
                Quantity = 2,
                UserId = 1,
                User = null!,
                Product = null!
            };

            var dto = new PartialUpdateCartItemDto
            {
                Quantity = 3
            };

            var result = dto.ToCartItem(entity);

            Assert.NotNull(result);
            Assert.IsType<CartItem>(result);
            Assert.Equal(0, result.Id);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(3, result.Quantity);
            Assert.Equal(1, result.UserId);
            Assert.Same(result, entity);
        }

        [Fact]
        public void ToGetCartItemDto_CartItem_ReturnsGetCartItemDto()
        {
            var entity = new CartItem
            {
                Id = 0,
                ProductId = 1,
                Quantity = 2,
                UserId = 1,
                User = null!,
                Product = null!
            };

            var result = entity.ToGetCartItemDto();

            Assert.NotNull(result);
            Assert.IsType<GetCartItemDto>(result);
            Assert.Equal(0, result.Id);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(2, result.Quantity);
            Assert.Equal(1, result.UserId);
        }
    }
}
