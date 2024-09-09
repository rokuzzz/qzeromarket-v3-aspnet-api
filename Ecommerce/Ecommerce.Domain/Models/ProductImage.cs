using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Common;

namespace Ecommerce.Domain.Models
{
    public class ProductImage : BaseEntity
    {
        public int ProductId { get; set; }
        [Column(TypeName = "varchar(255)")]
        public required string Url { get; set; }
        [Column(TypeName = "varchar(255)")]
        public required string Alt { get; set; }
    }
}
