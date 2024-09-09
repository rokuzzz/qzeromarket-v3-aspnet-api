using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Common
{
    public class TokenOptions
    {
        public int Id { get; set; }
        public Role Role { get; set; }
        public required string Email { get; set; }
    }
}