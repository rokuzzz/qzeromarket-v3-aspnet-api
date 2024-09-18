namespace Ecommerce.Domain.Filters
{
    public class ProductFilterOptions : PaginationOptionsBase
    {
        public int? CategoryId { get; set; }
    }
}