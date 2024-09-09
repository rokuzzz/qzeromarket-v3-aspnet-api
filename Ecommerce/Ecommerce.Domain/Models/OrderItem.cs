using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Common;

namespace Ecommerce.Domain.Models
{
    public class OrderItem : BaseEntity
    {
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "int")]
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        // Navigation properties
        public required Product Product { get; set; }
        public required Order Order { get; set; }
    }
}
