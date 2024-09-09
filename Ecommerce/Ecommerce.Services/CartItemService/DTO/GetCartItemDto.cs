using Ecommerce.Domain.Models;
using Ecommerce.Services.Common.DTO;

namespace Ecommerce.Services.CartItemService.DTO
{
    public class GetCartItemDto : IReadDto<CartItem>
    {
        public int Id { get; set; }
        public required int Quantity { get; set; }
        public required int UserId { get; set; }
        public required int ProductId { get; set; }

        public void FromEntity(CartItem entity)
        {
            Id = entity.Id;
            Quantity = entity.Quantity;
            UserId = entity.UserId;
            ProductId = entity.ProductId;
        }
    }
}