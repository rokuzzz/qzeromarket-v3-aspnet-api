using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Domain.Models
{
    public enum Role
    {
        Admin,
        User
    }

    [Index(nameof(Email), IsUnique = true)]
    public class User : BaseEntity
    {
        [Column(TypeName = "varchar(256)")]
        public required string Email { get; set; }
        [Column(TypeName = "varchar(100)")]
        public required string FirstName { get; set; }
        [Column(TypeName = "varchar(100)")]
        public required string LastName { get; set; }
        [Column(TypeName = "varchar(256)")]
        public required string Password { get; set; }
        [Column(TypeName = "varchar(256)")]
        public Role Role { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string? Avatar { get; set; }

        // Navigation properties
        public ICollection<Review> Reviews { get; set; }
        public required ICollection<CartItem> CartItems { get; set; }
        public required ICollection<Order> Orders { get; set; }
        public required SaltUser SaltUser { get; set; }


        public override string ToString()
        {
            return $"{FirstName} {LastName} ({Email}) - {Role}";
        }
    }
}