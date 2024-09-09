using Ecommerce.Domain.Common;
using Ecommerce.Services.Common.DTO;

namespace Ecommerce.Services.Common
{
    public class PaginatedResult<T, TReadDto>
        where T : BaseEntity
        where TReadDto : IReadDto<T>
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<TReadDto> Items { get; set; } = [];
    }
}