using Ecommerce.Domain.Models;

namespace Ecommerce.Domain.Filters
{
    public class UserFilterOptions : PaginationOptionsBase
    {
        public Role? Role { get; set; }
    }
}