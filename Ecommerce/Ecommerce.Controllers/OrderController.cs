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
    public class OrderController(IOrderService orderService, IOrderRepo orderRepo, IAuthorizationService authorizationService) : AppController<Order, OrderFilterOptions, GetOrderDto>(orderService)
    {
        private readonly IOrderService _orderService = orderService;
        private readonly IOrderRepo _orderRepo = orderRepo;
        private readonly IAuthorizationService _authorizationService = authorizationService;

        [HttpGet]
        [Produces("application/json")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Get all orders", Description = "Permission: Admin only")]
        [SwaggerResponse(200, "The orders were retrieved successfully", typeof(PaginatedResult<Order, GetOrderDto>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "User is not an admin", typeof(ProblemDetails))]
        async public override Task<ActionResult<PaginatedResult<Order, GetOrderDto>>> GetItems([FromQuery] OrderFilterOptions filteringOptions)
        {
            return await base.GetItems(filteringOptions);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get an order by id", Description = "Permission: Admin or owner of the order")]
        [SwaggerResponse(200, "The order was retrieved successfully", typeof(GetOrderDto))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "User is not the owner of the order or not an admin", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The order was not found", typeof(ProblemDetails))]
        async public override Task<ActionResult<Order>> GetItem(int id)
        {
            var entity = await _orderRepo.GetByIdAsync(id);
            var result = await _authorizationService.AuthorizeAsync(HttpContext.User, entity, "Ownership");
            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("User is not the owner of the order");
            }
            return await base.GetItem(id);
        }

        [HttpPost]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Create a new order", Description = "Permission: Authenticated users")]
        [SwaggerResponse(201, "The order was created successfully", typeof(PaginatedResult<Order, GetOrderDto>))]
        [SwaggerResponse(400, "Unsufficient stock or user doesn't have a cart", typeof(ProblemDetails))]
        [SwaggerResponse(400, "Validation error", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "User in the token is different from the user in the request", typeof(ProblemDetails))]
        public async Task<ActionResult<GetOrderDto>> CreateAsync([FromBody] CreateOrderDto dto)
        {
            var claimId = HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            var claimRole = HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Role);
            if (claimId?.Value != dto.UserId.ToString() && claimRole?.Value != "Admin")
            {
                throw new UnauthorizedAccessException("You are not allowed to create an order for another user");
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
        [SwaggerResponse(403, "User in the token is different from the user in the request", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The order was not found", typeof(ProblemDetails))]
        async public override Task<ActionResult<Order>> DeleteItem(int id)
        {
            return await base.DeleteItem(id);
        }
    }
}