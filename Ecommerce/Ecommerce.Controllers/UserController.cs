using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;
using Ecommerce.Services.UserService.DTO;
using Ecommerce.Services.UserService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using FluentValidation.Results;
using Ecommerce.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Domain.Interfaces;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    [SwaggerTag("Read, update and delete users")]
    public class UserController(IUserService userService, IAuthorizationService authorizationService, IUserRepo userRepo) : AppController<User, UserFilterOptions, GetUserDto>(userService)
    {
        private readonly IUserService _userService = userService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly IUserRepo _userRepo = userRepo;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a list of users", Description = "Permission: Admin")]
        [SwaggerResponse(200, "The users were fetched successfully", typeof(PaginatedResult<User, GetUserDto>))]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        public override async Task<ActionResult<PaginatedResult<User, GetUserDto>>> GetItems([FromQuery] UserFilterOptions filteringOptions)
        {
            return await base.GetItems(filteringOptions);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a user by id", Description = "Permission: Admin")]
        [SwaggerResponse(200, "The user was fetched successfully", typeof(GetUserDto))]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The user was not found", typeof(ProblemDetails))]
        public override async Task<ActionResult<User>> GetItem([SwaggerParameter(Description = "User id")] int id)
        {
            return await base.GetItem(id);
        }

        [HttpGet("me")]
        [Authorize]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get the current user's data", Description = "Permission: Any authenticated user")]
        [SwaggerResponse(200, "The user data was fetched successfully", typeof(GetUserDto))]
        [SwaggerResponse(404, "The user was not found", typeof(ProblemDetails))]
        public async Task<ActionResult<GetUserDto>> GetCurrentUser()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
            {
                return Unauthorized();
            }

            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new ProblemDetails { Title = "User not found" });
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update an existing user", Description = "Permission: Admin or owner of the user")]
        [SwaggerResponse(200, "The user was updated successfully", typeof(GetUserDto))]
        [SwaggerResponse(400, "Validation failed", typeof(List<ValidationFailure>))]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The user was not found", typeof(ProblemDetails))]
        public async Task<ActionResult<GetUserDto>> UpdateAsync([SwaggerParameter(Description = "User id")] int id, [FromBody] UpdateUserDto dto)
        {
            var validationResult = new UpdateUserDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var entity = await _userRepo.GetByIdAsync(id);
            var result = await _authorizationService.AuthorizeAsync(HttpContext.User, entity, "Ownership");

            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("User is not the owner of the cart item");
            }

            var updatedUser = await _userService.UpdateAsync(dto, id);
            return Ok(updatedUser);
        }

        [HttpPatch("{id}")]
        [Authorize]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Partially update an existing user", Description = "Permission: Admin")]
        [SwaggerResponse(200, "The user was updated successfully", typeof(GetUserDto))]
        [SwaggerResponse(400, "Validation failed", typeof(List<ValidationFailure>))]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The user was not found", typeof(ProblemDetails))]
        public async Task<ActionResult<GetUserDto>> PartialUpdateAsync([SwaggerParameter(Description = "User id")] int id, [FromBody] PartialUpdateUserDto dto)
        {
            var validationResult = new PartialUpdateUserDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var updatedUser = await _userService.PartialUpdateByIdAsync(dto, id);
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Delete a user by id", Description = "Permission: Admin")]
        [SwaggerResponse(204, "The user was deleted successfully")]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The user was not found", typeof(ProblemDetails))]
        public override async Task<ActionResult<User>> DeleteItem(int id)
        {
            return await base.DeleteItem(id);
        }
    }
}