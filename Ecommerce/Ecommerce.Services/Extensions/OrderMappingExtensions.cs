using Ecommerce.Domain.Models;
using Ecommerce.Services.OrderService.DTO;

namespace Ecommerce.Services.Extensions
{
    public static class OrderMappingExtensions
    {
        public static Order ToOrder(this CreateOrderDto dto)
        {
            return new Order
            {
                Id = 0,
                UserId = dto.UserId,
                OrderDate = DateTime.UtcNow,
                User = null!,
                OrderItems = []
            };
        }


        public static GetOrderDto ToGetOrderDto(this Order entity)
        {
            return new GetOrderDto
            {
                Id = entity.Id,
                OrderDate = entity.OrderDate,
                UserId = entity.UserId,
                OrderItems = entity.OrderItems?.Select(oi => new OrderItemDto
                {
                    Quantity = oi.Quantity,
                    ProductId = oi.ProductId
                }).ToList() ?? []
            };
        }
    }
}