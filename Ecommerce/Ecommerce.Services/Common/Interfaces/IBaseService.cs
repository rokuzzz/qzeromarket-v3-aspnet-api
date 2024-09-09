using Ecommerce.Domain.Common;
using Ecommerce.Domain.Filters;
using Ecommerce.Services.Common.DTO;

namespace Ecommerce.Services.Common.Interfaces
{
    public interface IBaseService<T, TFilter, TReadDto>
    where T : BaseEntity
    where TFilter : PaginationOptionsBase
    where TReadDto : IReadDto<T>
    {
        Task<PaginatedResult<T, TReadDto>> GetAllAsync(TFilter filteringOptions);
        Task<TReadDto> GetByIdAsync(int id);
        Task<bool> DeleteByIdAsync(int id);
    }
}