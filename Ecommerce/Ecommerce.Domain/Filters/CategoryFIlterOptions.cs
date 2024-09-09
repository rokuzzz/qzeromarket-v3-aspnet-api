namespace Ecommerce.Domain.Filters
{
    public class CategoryFilterOptions : PaginationOptionsBase
    {
        public int? ParentCategoryId { get; set; }
    }
}