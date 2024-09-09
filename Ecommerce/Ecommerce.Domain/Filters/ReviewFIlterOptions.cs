namespace Ecommerce.Domain.Filters
{
    public class ReviewFilterOptions : PaginationOptionsBase
    {
        public required int ProductId { get; set; }
    }
}