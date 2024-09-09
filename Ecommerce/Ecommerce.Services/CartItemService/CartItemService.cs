using Ecommerce.Domain.Common.Exceptions;
using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Services.CartItemService.DTO;
using Ecommerce.Services.CartItemService.Interfaces;
using Ecommerce.Services.Common;
using Ecommerce.Services.Extensions;
using Npgsql;

namespace Ecommerce.Services.CartItemService
{
    public class CartItemService(ICartItemRepo cartItemRepo) : BaseService<CartItem, CartItemFilterOptions, GetCartItemDto>(cartItemRepo), ICartItemService
    {
        private readonly ICartItemRepo _repo = cartItemRepo;
        public async Task<GetCartItemDto> CreateAsync(CreateCartItemDto createCartItemDto)
        {
            var result = await _repo.CreateAsync(createCartItemDto.ToCartItem());
            return result.ToGetCartItemDto();
        }

        public async Task<GetCartItemDto> PartialUpdateAsync(PartialUpdateCartItemDto dto, int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            var updatedEntity = dto.ToCartItem(entity);
            var result = await _repo.PartialUpdateByIdAsync(updatedEntity, id);
            return result.ToGetCartItemDto();
        }
    }
}