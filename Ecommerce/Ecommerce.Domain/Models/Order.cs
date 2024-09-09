using Ecommerce.Domain.Common;

namespace Ecommerce.Domain.Models
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        public required User User { get; set; }
        public required ICollection<OrderItem> OrderItems { get; set; }
    }
}
