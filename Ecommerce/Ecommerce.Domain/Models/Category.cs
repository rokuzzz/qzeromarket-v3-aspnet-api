using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Domain.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category : BaseEntity
    {
        [Column(TypeName = "varchar(50)")]
        public required string Name { get; set; }
        [Column(TypeName = "varchar(100)")]
        public required string CategoryImage { get; set; }
        public int? ParentCategoryId { get; set; }

        // Navigation properties
        public Category? ParentCategory { get; set; }
        public required ICollection<Category> SubCategories { get; set; }
        public required ICollection<Product> Products { get; set; }
    }
}
