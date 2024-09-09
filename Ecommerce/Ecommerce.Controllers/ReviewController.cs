using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;
using Ecommerce.Services.ReviewService.DTO;
using Ecommerce.Services.ReviewService.Interfaces;
using Ecommerce.Services.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Services.Common;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/v1/reviews")]
    [SwaggerTag("Create, read, update and delete reviews")]
    public class ReviewController(IReviewService reviewService) : AppController<Review, ReviewFilterOptions, GetReviewDto>(reviewService)
    {
        private readonly IReviewService _reviewService = reviewService;

        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a list of reviews", Description = "Permission: All users")]
        [SwaggerResponse(200, "The reviews were fetched successfully", typeof(PaginatedResult<Review, GetReviewDto>))]
        [SwaggerResponse(401, "Unauthorized")]
        async public override Task<ActionResult<PaginatedResult<Review, GetReviewDto>>> GetItems([FromQuery] ReviewFilterOptions filteringOptions)
        {
            return await base.GetItems(filteringOptions);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a review by id", Description = "Permission: All users")]
        [SwaggerResponse(200, "The review was fetched successfully", typeof(GetReviewDto))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(404, "The review was not found", typeof(ProblemDetails))]
        async public override Task<ActionResult<Review>> GetItem(int id)
        {
            return await base.GetItem(id);
        }

        [HttpPost]
        [Authorize]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Create a new review", Description = "Permission: Users with valid token")]
        [SwaggerResponse(201, "The review was created successfully", typeof(GetReviewDto))]
        [SwaggerResponse(400, "Validation failed", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        [SwaggerResponse(409, "The review already exists", typeof(ProblemDetails))]
        public async Task<ActionResult<GetReviewDto>> CreateAsync([FromBody] CreateOrUpdateReviewDto dto)
        {
            var validationResult = new CreateOrUpdateReviewDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var entity = await _reviewService.UpsertAsync(dto);

            return CreatedAtAction(nameof(GetItem), new { id = entity.Id }, entity);
        }

        [HttpPatch("{id}")]
        [Authorize]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update an existing review", Description = "Permission: Users with valid token")]
        [SwaggerResponse(200, "The review was updated successfully", typeof(GetReviewDto))]
        [SwaggerResponse(400, "Validation failed", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The review was not found", typeof(ProblemDetails))]
        [SwaggerResponse(409, "The review already exists", typeof(ProblemDetails))]
        public async Task<ActionResult<GetReviewDto>> PartialUpdateAsync(int id, [FromBody] PartialUpdateReviewDto dto)
        {
            var validationResult = new PartialUpdateReviewDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var entity = await _reviewService.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            var updatedEntity = await _reviewService.PartialUpdateByIdAsync(dto, id);

            return Ok(updatedEntity);
        }

        [HttpPut("{id}")]
        [Authorize]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update an existing review", Description = "Permission: Users with valid token")]
        [SwaggerResponse(200, "The review was updated successfully", typeof(GetReviewDto))]
        [SwaggerResponse(201, "The review was created successfully", typeof(GetReviewDto))]
        [SwaggerResponse(400, "Validation failed", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The review was not found", typeof(ProblemDetails))]
        [SwaggerResponse(409, "The review already exists", typeof(ProblemDetails))]
        public async Task<ActionResult<GetReviewDto>> UpdateAsync(int id, [FromBody] CreateOrUpdateReviewDto dto)
        {
            var validationResult = new CreateOrUpdateReviewDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var entity = await _reviewService.UpsertAsync(dto, id);
            if (entity.Id != id)
            {
                return CreatedAtAction(nameof(GetItem), new { id = entity.Id }, entity);
            }
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Delete a review by id", Description = "Permission: Users with valid token")]
        [SwaggerResponse(204, "The review was deleted successfully")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The review was not found", typeof(ProblemDetails))]
        public override async Task<ActionResult<Review>> DeleteItem(int id)
        {
            return await base.DeleteItem(id);
        }
    }
}
