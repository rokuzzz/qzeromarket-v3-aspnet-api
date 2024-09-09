using Ecommerce.Domain.Common;
using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Services.Common.DTO;
using Ecommerce.Services.Common.Interfaces;

namespace Ecommerce.Services.Common
{
    public class BaseService<T, TFilter, TReadDto>(IBaseRepo<T, TFilter> repo) : IBaseService<T, TFilter, TReadDto>
        where T : BaseEntity
        where TFilter : PaginationOptionsBase
        where TReadDto : IReadDto<T>
    {
        private readonly IBaseRepo<T, TFilter> _repo = repo;

        public async Task<bool> DeleteByIdAsync(int id)
        {
            return await _repo.DeleteByIdAsync(id);
        }

        public async Task<PaginatedResult<T, TReadDto>> GetAllAsync(TFilter filteringOptions)
        {
            var total = await _repo.CountAsync(filteringOptions);
            var query = await _repo.GetAllAsync(filteringOptions);
            var result = new List<TReadDto>();
            foreach (var entity in query)
            {
                var dto = Activator.CreateInstance<TReadDto>();

                dto.FromEntity(entity);
                result.Add(dto);
            }
            var paginatedResult = new PaginatedResult<T, TReadDto>
            {
                ItemsPerPage = filteringOptions.PerPage ?? 10,
                CurrentPage = filteringOptions.Page ?? 1,
                TotalItems = total,
                Items = result
            };

            return paginatedResult;
        }

        public async Task<TReadDto> GetByIdAsync(int id)
        {
            var query = await _repo.GetByIdAsync(id);
            var dto = Activator.CreateInstance<TReadDto>();
            dto.FromEntity(query);
            return dto;
        }
    }
}