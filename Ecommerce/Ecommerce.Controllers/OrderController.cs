using System.Security.Claims;
using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Services.Common;
using Ecommerce.Services.OrderService.DTO;
using Ecommerce.Services.OrderService.Interfaces;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/orders")]
    [SwaggerTag("Create, read, update and delete orders")]
    public class OrderController : AppController<Order, OrderFilterOptions, GetOrderDto>
    {
        private readonly IOrderService _orderService;
        private readonly IOrderRepo _orderRepo;

        public OrderController(
            IOrderService orderService,
            IOrderRepo orderRepo,
            IAuthorizationService authorizationService)
            : base(orderService)
        {
            _orderService = orderService;
            _orderRepo = orderRepo;
        }

        [HttpGet]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get orders", Description = "Permission: Authenticated users can get their own orders, Admins can get all orders")]
        [SwaggerResponse(200, "The orders were retrieved successfully", typeof(PaginatedResult<Order, GetOrderDto>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        public async override Task<ActionResult<PaginatedResult<Order, GetOrderDto>>> GetItems([FromQuery] OrderFilterOptions filteringOptions)
        {
            var user = HttpContext.User;
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            var roleClaim = user.FindFirst(ClaimTypes.Role);
            var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : (int?)null;
            var isAdmin = roleClaim != null && roleClaim.Value == "Admin";

            if (isAdmin)
            {
                return await base.GetItems(filteringOptions);
            }
            else
            {
                if (userId == null)
                {
                    return Unauthorized("User ID not found in token");
                }

                filteringOptions.UserId = userId;

                return await base.GetItems(filteringOptions);
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get an order by id", Description = "Permission: Admin or owner of the order")]
        [SwaggerResponse(200, "The order was retrieved successfully", typeof(GetOrderDto))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The order was not found", typeof(ProblemDetails))]
        public async override Task<ActionResult<Order>> GetItem(int id)
        {
            var user = HttpContext.User;
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            var roleClaim = user.FindFirst(ClaimTypes.Role);
            var userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : (int?)null;
            var isAdmin = roleClaim != null && roleClaim.Value == "Admin";

            var order = await _orderRepo.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            if (isAdmin || (userId.HasValue && order.UserId == userId.Value))
            {
                return Ok(order);
            }
            else
            {
                return Forbid("You are not authorized to access this order");
            }
        }

        [HttpPost]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Create a new order", Description = "Permission: Authenticated users")]
        [SwaggerResponse(201, "The order was created successfully", typeof(GetOrderDto))]
        [SwaggerResponse(400, "Insufficient stock or user doesn't have a cart", typeof(ProblemDetails))]
        [SwaggerResponse(400, "Validation error", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        public async Task<ActionResult<GetOrderDto>> CreateAsync([FromBody] CreateOrderDto dto)
        {
            var claimId = HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            var claimRole = HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Role);

            if (claimId == null)
            {
                return Unauthorized("User ID not found in token");
            }

            var userId = int.Parse(claimId.Value);

            if (userId != dto.UserId && (claimRole == null || claimRole.Value != "Admin"))
            {
                return Forbid("You are not allowed to create an order for another user");
            }

            var validationResult = new CreateOrderDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var entity = await _orderService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetItem), new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Delete an order by id", Description = "Permission: Admin only")]
        [SwaggerResponse(204, "The order was deleted successfully")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The order was not found", typeof(ProblemDetails))]
        public async override Task<ActionResult<Order>> DeleteItem(int id)
        {
            return await base.DeleteItem(id);
        }
    }
}
