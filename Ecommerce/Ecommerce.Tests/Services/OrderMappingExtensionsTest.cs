using Ecommerce.Domain.Models;
using Ecommerce.Services.OrderService.DTO;
using Ecommerce.Services.Extensions;

namespace Ecommerce.Tests.Services
{
    public class OrderMappingExtensionsTest

    {
        [Fact]
        public void ToOrder_CreateOrderDto_ReturnsNewOrder()
        {
            var dto = new CreateOrderDto
            {
                UserId = 1
            };

            var result = dto.ToOrder();

            Assert.NotNull(result);
            Assert.IsType<Order>(result);
            Assert.Equal(0, result.Id);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public void ToGetOrderDto_Order_ReturnsGetOrderDto()
        {
            var entity = new Order
            {
                Id = 0,
                UserId = 1,
                OrderItems = [new OrderItem{
                    Quantity = 1,
                    ProductId = 1,
                    Product = null!,
                    Order = null!
                }],
                User = null!
            };

            var result = entity.ToGetOrderDto();

            Assert.NotNull(result);
            Assert.IsType<GetOrderDto>(result);
            Assert.Equal(0, result.Id);
            Assert.Equal(1, result.UserId);
            Assert.IsType<List<OrderItemDto>>(result.OrderItems);
        }
    }
}
