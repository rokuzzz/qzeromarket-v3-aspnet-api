using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Common;

namespace Ecommerce.Domain.Models
{
    public class Review : BaseEntity
    {
        [Column(TypeName = "varchar(255)")]
        public required string Title { get; set; }

        [Column(TypeName = "varchar(500)")]
        public required string Description { get; set; }

        public int Rating { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }

        public User? User { get; set; }
        public Product? Product { get; set; }
    }
}