using Ecommerce.Domain.Common.Exceptions;
using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Services.Common;
using Ecommerce.Services.Extensions;
using Ecommerce.Services.OrderService.DTO;
using Ecommerce.Services.OrderService.Interfaces;
using Npgsql;

namespace Ecommerce.Services.OrderService
{
    public class OrderService(IOrderRepo orderRepo) : BaseService<Order, OrderFilterOptions, GetOrderDto>(orderRepo), IOrderService
    {
        private readonly IOrderRepo _repo = orderRepo;
        public async Task<GetOrderDto> CreateAsync(CreateOrderDto createOrderDto)
        {
            var result = await _repo.CreateAsync(createOrderDto.ToOrder());
            return result.ToGetOrderDto();
        }
    }
}