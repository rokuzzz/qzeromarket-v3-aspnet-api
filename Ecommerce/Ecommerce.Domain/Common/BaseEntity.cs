using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Domain.Common
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}