namespace Ecommerce.Domain.Filters
{
    public class CartItemFilterOptions : PaginationOptionsBase
    {
        public required int UserId { get; set; }
    }
}