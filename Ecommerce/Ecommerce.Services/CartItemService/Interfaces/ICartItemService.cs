using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;
using Ecommerce.Services.CartItemService.DTO;
using Ecommerce.Services.Common.Interfaces;

namespace Ecommerce.Services.CartItemService.Interfaces
{
    public interface ICartItemService : IBaseService<CartItem, CartItemFilterOptions, GetCartItemDto>
    {
        Task<GetCartItemDto> CreateAsync(CreateCartItemDto createCartItemDto);
        Task<GetCartItemDto> PartialUpdateAsync(PartialUpdateCartItemDto partialUpdateCartItemDto, int id);
    }
}