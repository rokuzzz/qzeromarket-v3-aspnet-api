using System.Security.Claims;
using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Services.CartItemService.DTO;
using Ecommerce.Services.CartItemService.Interfaces;
using Ecommerce.Services.Common;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/cart-items")]
    [SwaggerTag("Create, read, update and delete cart items")]
    public class CartItemController(ICartItemService cartItemService, IAuthorizationService authorizationService, ICartItemRepo cartItemRepo) : AppController<CartItem, CartItemFilterOptions, GetCartItemDto>(cartItemService)
    {
        private readonly ICartItemService _cartItemService = cartItemService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ICartItemRepo _cartItemRepo = cartItemRepo;

        [HttpGet]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a list of cart items", Description = "Permission: Admin or owner of the cart items")]
        [SwaggerResponse(200, "The cart items were fetched successfully", typeof(PaginatedResult<CartItem, GetCartItemDto>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "User is not the owner of the cart items or not an admin", typeof(ProblemDetails))]
        public override async Task<ActionResult<PaginatedResult<CartItem, GetCartItemDto>>> GetItems([FromQuery] CartItemFilterOptions filteringOptions)
        {
            var entities = await _cartItemRepo.GetAllAsync(filteringOptions);
            foreach (var entity in entities)
            {
                var result = await _authorizationService.AuthorizeAsync(HttpContext.User, entity, "Ownership");

                if (!result.Succeeded)
                {
                    throw new UnauthorizedAccessException("User is not the owner of the cart items");
                }
            }

            return await base.GetItems(filteringOptions);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a cart item by id", Description = "Permission: Admin or owner of the cart items")]
        [SwaggerResponse(200, "The cart item was updated successfully", typeof(GetCartItemDto))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "User is not the owner of the cart item or not an admin", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The cart item was not found", typeof(ProblemDetails))]
        public override async Task<ActionResult<CartItem>> GetItem(int id)
        {

            var entity = await _cartItemRepo.GetByIdAsync(id);
            var result = await _authorizationService.AuthorizeAsync(HttpContext.User, entity, "Ownership");

            if (result.Succeeded)
            {
                return await base.GetItem(id);
            }
            throw new UnauthorizedAccessException("User is not the owner of the cart item");
        }

        [HttpPost]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Create a new cart item", Description = "Permission: Authenticated users")]
        [SwaggerResponse(201, "The cart item was created successfully", typeof(GetCartItemDto))]
        [SwaggerResponse(400, "Validation failed", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "User in the token is different from the user in the request", typeof(ProblemDetails))]
        [SwaggerResponse(404, "Product or user don't exist", typeof(ProblemDetails))]
        public async Task<ActionResult<GetCartItemDto>> CreateAsync([FromBody] CreateCartItemDto dto)
        {
            var claimId = HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            var claimRole = HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Role);
            if (claimId?.Value != dto.UserId.ToString() && claimRole?.Value != "Admin")
            {
                throw new UnauthorizedAccessException("You are not allowed to create a cart item for another user");
            }
            var validationResult = new CreateCartItemDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var entity = await _cartItemService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetItem), new { id = entity.Id }, entity);
        }

        [HttpPatch("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update an existing cart item", Description = "Permission: Admin or owner of the cart items")]
        [SwaggerResponse(200, "The cart item was updated successfully", typeof(GetCartItemDto))]
        [SwaggerResponse(400, "Validation failed", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "User is not the owner of the cart item or not an admin", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The cart item was not found", typeof(ProblemDetails))]
        public async Task<ActionResult<GetCartItemDto>> PartialUpdateAsync([SwaggerParameter(description: "Cart item id")] int id, [FromBody] PartialUpdateCartItemDto dto)
        {
            var validationResult = new PartialUpdateCartItemDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var entity = await _cartItemRepo.GetByIdAsync(id);
            var result = await _authorizationService.AuthorizeAsync(HttpContext.User, entity, "Ownership");
            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("User is not the owner of the cart item");
            }

            var updatedEntity = await _cartItemService.PartialUpdateAsync(dto, id);

            return Ok(updatedEntity);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a cart item by id", Description = "Permission: Admin or owner of the cart items")]
        [SwaggerResponse(204, "The cart item was deleted successfully")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "User is not the owner of the cart item or not an admin", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The cart item was not found", typeof(ProblemDetails))]
        public override async Task<ActionResult<CartItem>> DeleteItem(int id)
        {
            var entity = await _cartItemRepo.GetByIdAsync(id);
            var result = await _authorizationService.AuthorizeAsync(HttpContext.User, entity, "Ownership");
            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("User is not the owner of the cart item");
            }

            return await base.DeleteItem(id);
        }
    }
}