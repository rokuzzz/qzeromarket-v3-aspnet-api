using Ecommerce.Domain.Common;
using Ecommerce.Domain.Filters;
using Ecommerce.Services.Common;
using Ecommerce.Services.Common.DTO;
using Ecommerce.Services.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]s")]
    public class AppController<T, TFilter, TReadDto>(IBaseService<T, TFilter, TReadDto> baseService) : ControllerBase
    where T : BaseEntity
    where TFilter : PaginationOptionsBase
    where TReadDto : IReadDto<T>
    {
        private readonly IBaseService<T, TFilter, TReadDto> _baseService = baseService;

        [Authorize]
        [HttpGet]
        [Produces("application/json")]
        public virtual async Task<ActionResult<PaginatedResult<T, TReadDto>>> GetItems([FromQuery] TFilter filteringOptions)
        {
            var items = await _baseService.GetAllAsync(filteringOptions);
            return Ok(items);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public virtual async Task<ActionResult<T>> GetItem(int id)
        {
            var item = await _baseService.GetByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<T>> DeleteItem(int id)
        {
            await _baseService.DeleteByIdAsync(id);

            return NoContent();
        }
    }
};