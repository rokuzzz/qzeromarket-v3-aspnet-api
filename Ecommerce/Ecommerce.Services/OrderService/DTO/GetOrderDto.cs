using Ecommerce.Domain.Models;
using Ecommerce.Services.Common.DTO;

namespace Ecommerce.Services.OrderService.DTO
{
    public class OrderItemDto
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }

    public class GetOrderDto : IReadDto<Order>
    {
        public int Id { get; set; }
        public required int UserId { get; set; }
        public required DateTime OrderDate { get; set; }
        public required ICollection<OrderItemDto> OrderItems { get; set; }

        public void FromEntity(Order entity)
        {
            Id = entity.Id;
            UserId = entity.UserId;
            OrderDate = entity.OrderDate;
            OrderItems = entity.OrderItems?.Select(oi => new OrderItemDto
            {
                Quantity = oi.Quantity,
                ProductId = oi.ProductId
            }).ToList() ?? [];
        }
    }
}