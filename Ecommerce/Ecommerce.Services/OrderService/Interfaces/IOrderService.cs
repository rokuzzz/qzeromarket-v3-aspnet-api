using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;
using Ecommerce.Services.OrderService.DTO;
using Ecommerce.Services.Common.Interfaces;

namespace Ecommerce.Services.OrderService.Interfaces
{
    public interface IOrderService : IBaseService<Order, OrderFilterOptions, GetOrderDto>
    {
        Task<GetOrderDto> CreateAsync(CreateOrderDto createOrderDto);
    }
}