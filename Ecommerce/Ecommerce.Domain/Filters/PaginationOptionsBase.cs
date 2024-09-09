namespace Ecommerce.Domain.Filters
{
    public abstract class PaginationOptionsBase
    {
        public int? Page { get; set; } = 1;
        public int? PerPage { get; set; } = 10;
    }
}