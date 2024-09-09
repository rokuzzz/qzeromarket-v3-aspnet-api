using Moq;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Services.CartItemService;
using Ecommerce.Services.CartItemService.DTO;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Common.Exceptions;
using Ecommerce.Domain.Filters;
using Ecommerce.Services.Common;

namespace Ecommerce.Tests.Services
{
    public class CartItemServiceTest
    {
        private readonly Mock<ICartItemRepo> _mockCartItemRepo;
        private readonly CartItemService _cartItemService;

        public CartItemServiceTest()
        {
            _mockCartItemRepo = new Mock<ICartItemRepo>();
            _cartItemService = new CartItemService(_mockCartItemRepo.Object);
        }

        [Fact]
        public async Task CreateAsync_WhenCalled_ReturnsGetDto()
        {
            _mockCartItemRepo
                .Setup(repo => repo.CreateAsync(It.IsAny<CartItem>()))
                .ReturnsAsync((CartItem cartItem) => cartItem);

            var dto = new CreateCartItemDto
            {
                ProductId = 1,
                Quantity = 2,
                UserId = 1
            };

            var result = await _cartItemService.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.IsType<GetCartItemDto>(result);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(2, result.Quantity);
            Assert.Equal(1, result.UserId);

            _mockCartItemRepo.Verify(repo => repo.CreateAsync(It.IsAny<CartItem>()), Times.Once);
        }

        [Fact]
        public async Task PartialUpdateAsync_WhenCalledWithPartialUpdateDto_ReturnsGetDto()
        {
            var cartItem = new CartItem
            {
                Id = 2,
                ProductId = 1,
                Quantity = 2,
                UserId = 1,
                User = null!,
                Product = null!
            };

            _mockCartItemRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(cartItem);

            _mockCartItemRepo
                .Setup(repo => repo.PartialUpdateByIdAsync(It.IsAny<CartItem>(), It.IsAny<int>()))
                .ReturnsAsync((CartItem cartItem, int id) => cartItem);

            var dto = new PartialUpdateCartItemDto
            {
                Quantity = 3
            };

            var result = await _cartItemService.PartialUpdateAsync(dto, 1);

            Assert.NotNull(result);
            Assert.IsType<GetCartItemDto>(result);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(3, result.Quantity);
            Assert.Equal(1, result.UserId);

            _mockCartItemRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _mockCartItemRepo.Verify(repo => repo.PartialUpdateByIdAsync(cartItem, 1), Times.Once);
        }

        [Fact]
        public async Task PartialUpdateByIdAsync_WhenCalledWithPartialUpdateDtoAndCartItemNotFound_ThrowsException()
        {
            _mockCartItemRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new CartItemNotFoundExcepiton());

            var dto = new PartialUpdateCartItemDto
            {
                Quantity = 3
            };

            await Assert.ThrowsAsync<CartItemNotFoundExcepiton>(() => _cartItemService.PartialUpdateAsync(dto, 1));

            _mockCartItemRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _mockCartItemRepo.Verify(repo => repo.PartialUpdateByIdAsync(It.IsAny<CartItem>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenCalledWithExistingId_ReturnsTrue()
        {
            _mockCartItemRepo
                .Setup(repo => repo.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var result = await _cartItemService.DeleteByIdAsync(1);

            Assert.True(result);

            _mockCartItemRepo.Verify(repo => repo.DeleteByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenCalledWithWrongId_ReturnsFalse()
        {
            _mockCartItemRepo
                .Setup(repo => repo.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var result = await _cartItemService.DeleteByIdAsync(1);

            Assert.False(result);

            _mockCartItemRepo.Verify(repo => repo.DeleteByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCalledWithExistingId_ReturnsGetDtoAsync()
        {
            var cartItem = new CartItem
            {
                Id = 1,
                ProductId = 1,
                Quantity = 2,
                UserId = 1,
                User = null!,
                Product = null!
            };

            _mockCartItemRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(cartItem);

            var result = await _cartItemService.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.IsType<GetCartItemDto>(result);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(2, result.Quantity);
            Assert.Equal(1, result.UserId);

            _mockCartItemRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCalledWithWrongId_ThrowsException()
        {
            _mockCartItemRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new CartItemNotFoundExcepiton());

            await Assert.ThrowsAsync<CartItemNotFoundExcepiton>(() => _cartItemService.GetByIdAsync(1));

            _mockCartItemRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenCalled_ReturnsPaginatedResult()
        {
            var cartItem = new CartItem
            {
                Id = 1,
                ProductId = 1,
                Quantity = 2,
                UserId = 1,
                User = null!,
                Product = null!
            };

            _mockCartItemRepo
                .Setup(repo => repo.GetAllAsync(It.IsAny<CartItemFilterOptions>()))
                .ReturnsAsync([cartItem]);

            _mockCartItemRepo
                .Setup(repo => repo.CountAsync(It.IsAny<CartItemFilterOptions>()))
                .ReturnsAsync(1);

            var filteringOptions = new CartItemFilterOptions
            {
                UserId = 1
            };

            var result = await _cartItemService.GetAllAsync(filteringOptions);

            Assert.NotNull(result);
            Assert.IsType<PaginatedResult<CartItem, GetCartItemDto>>(result);
            Assert.Equal(10, result.ItemsPerPage);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(1, result.TotalItems);
            Assert.Single(result.Items);
            Assert.Equal(1, result.Items.First().ProductId);
            Assert.Equal(2, result.Items.First().Quantity);
            Assert.Equal(1, result.Items.First().UserId);

            _mockCartItemRepo.Verify(repo => repo.GetAllAsync(filteringOptions), Times.Once);
        }
    }
}