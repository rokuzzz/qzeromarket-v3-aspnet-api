using Moq;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Services.OrderService;
using Ecommerce.Services.OrderService.DTO;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Common.Exceptions;
using Ecommerce.Domain.Filters;
using Ecommerce.Services.Common;
using Npgsql;

namespace Ecommerce.Tests.Services
{
    public class OrderServiceTest
    {
        private readonly Mock<IOrderRepo> _mockOrderRepo;
        private readonly OrderService _orderService;

        public OrderServiceTest()
        {
            _mockOrderRepo = new Mock<IOrderRepo>();
            _orderService = new OrderService(_mockOrderRepo.Object);
        }

        [Fact]
        public async Task CreateAsync_WhenCalled_ReturnsGetDto()
        {
            _mockOrderRepo
                .Setup(repo => repo.CreateAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order order) => order);

            var dto = new CreateOrderDto
            {
                UserId = 1,
            };

            var result = await _orderService.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.IsType<GetOrderDto>(result);
            Assert.Equal(1, result.UserId);

            _mockOrderRepo.Verify(repo => repo.CreateAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenInsufficientStock_ThrowsError()
        {
            _mockOrderRepo
                .Setup(repo => repo.CreateAsync(It.IsAny<Order>()))
                .ThrowsAsync(new InsufficientStockException());

            var dto = new CreateOrderDto
            {
                UserId = 1,
            };

            await Assert.ThrowsAsync<InsufficientStockException>(() => _orderService.CreateAsync(dto));
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenCalledWithExistingId_ReturnsTrue()
        {
            _mockOrderRepo
                .Setup(repo => repo.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var result = await _orderService.DeleteByIdAsync(1);

            Assert.True(result);

            _mockOrderRepo.Verify(repo => repo.DeleteByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenCalledWithWrongId_ReturnsFalse()
        {
            _mockOrderRepo
                .Setup(repo => repo.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var result = await _orderService.DeleteByIdAsync(1);

            Assert.False(result);

            _mockOrderRepo.Verify(repo => repo.DeleteByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCalledWithExistingId_ReturnsGetDtoAsync()
        {
            var order = new Order
            {
                Id = 1,
                UserId = 1,
                OrderItems = null!,
                User = null!
            };

            _mockOrderRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(order);

            var result = await _orderService.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.IsType<GetOrderDto>(result);
            Assert.Equal(1, result.UserId);



            _mockOrderRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCalledWithWrongId_ThrowsException()
        {
            _mockOrderRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new OrderNotFoundException());

            await Assert.ThrowsAsync<OrderNotFoundException>(() => _orderService.GetByIdAsync(1));

            _mockOrderRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenCalled_ReturnsPaginatedResult()
        {
            var order = new Order
            {
                Id = 1,
                UserId = 1,
                OrderItems = null!,
                User = null!
            };


            _mockOrderRepo
                .Setup(repo => repo.GetAllAsync(It.IsAny<OrderFilterOptions>()))
                .ReturnsAsync([order]);

            _mockOrderRepo
                .Setup(repo => repo.CountAsync(It.IsAny<OrderFilterOptions>()))
                .ReturnsAsync(1);

            var filteringOptions = new OrderFilterOptions { };

            var result = await _orderService.GetAllAsync(filteringOptions);

            Assert.NotNull(result);
            Assert.IsType<PaginatedResult<Order, GetOrderDto>>(result);
            Assert.Equal(10, result.ItemsPerPage);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(1, result.TotalItems);
            Assert.Single(result.Items);
            Assert.Equal(1, result.Items.First().UserId);

            _mockOrderRepo.Verify(repo => repo.GetAllAsync(filteringOptions), Times.Once);
        }
    }
}