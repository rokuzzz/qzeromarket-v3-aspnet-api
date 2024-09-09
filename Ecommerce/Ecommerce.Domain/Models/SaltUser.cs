using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Common;

namespace Ecommerce.Domain.Models
{
    public class SaltUser : BaseEntity
    {
        public required byte[] Salt { get; set; }
        public required User User { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
    }
}