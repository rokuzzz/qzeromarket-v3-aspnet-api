using Ecommerce.Domain.Common;

namespace Ecommerce.Domain.Models
{
    public class CartItem : BaseEntity
    {
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }

        // Navigation properties
        public required User User { get; set; }
        public required Product Product { get; set; }
    }
}
