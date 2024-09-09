using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;
using Ecommerce.Services.CategoryService.DTO;
using Ecommerce.Services.CategoryService.Interfaces;
using Ecommerce.Services.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using FluentValidation.Results;
using Ecommerce.Services.Common;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/v1/categories")]
    [SwaggerTag("Create, read, update and delete categories")]
    public class CategoryController(ICategoryService categoryService, IFileService fileService) : AppController<Category, CategoryFilterOptions, GetCategoryDto>(categoryService)
    {
        private readonly ICategoryService _categoryService = categoryService;
        private readonly IFileService _fileService = fileService;

        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a list of categories", Description = "Permission: All users")]
        [SwaggerResponse(200, "The categories were fetched successfully", typeof(PaginatedResult<Category, GetCategoryDto>))]
        [SwaggerResponse(401, "Unauthorized")]
        async public override Task<ActionResult<PaginatedResult<Category, GetCategoryDto>>> GetItems([FromQuery] CategoryFilterOptions filteringOptions)
        {
            return await base.GetItems(filteringOptions);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get a category by id", Description = "Permission: All users")]
        [SwaggerResponse(200, "The category was updated successfully", typeof(GetCategoryDto))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(404, "The category was not found", typeof(ProblemDetails))]
        async public override Task<ActionResult<Category>> GetItem(int id)
        {
            return await base.GetItem(id);
        }


        [HttpPost]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Create a new category", Description = "Permission: Admin only")]
        [SwaggerResponse(201, "The category was created successfully", typeof(GetCategoryDto))]
        [SwaggerResponse(400, "Validation failed", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        [SwaggerResponse(409, "The category already exists", typeof(ProblemDetails))]
        public async Task<ActionResult<GetCategoryDto>> CreateAsync([FromForm] CreateOrUpdateCategoryDto dto)
        {
            var validationResult = new CreateOrUpdateCategoryDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var relativeFilePath = await _fileService.SaveFileAsync(dto.CategoryImage, "category");
            dto.CategoryImagePath = relativeFilePath;
            var entity = await _categoryService.UpsertAsync(dto);

            return CreatedAtAction(nameof(GetItem), new { id = entity.Id }, entity);
        }

        [HttpPatch("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update an existing category", Description = "Permission: Admin only")]
        [SwaggerResponse(200, "The category was updated successfully", typeof(GetCategoryDto))]
        [SwaggerResponse(400, "Validation failed", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The category was not found", typeof(ProblemDetails))]
        [SwaggerResponse(409, "The category already exists", typeof(ProblemDetails))]
        public async Task<ActionResult<GetCategoryDto>> PartialUpdateAsync(int id, [FromForm] PartialUpdateCategoryDto dto)
        {
            var validationResult = new PartialUpdateCategoryDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (dto.CategoryImage != null)
            {
                var relativeFilePath = await _fileService.SaveFileAsync(dto.CategoryImage, "category");
                dto.CategoryImagePath = relativeFilePath;
            }

            var updatedEntity = await _categoryService.PartialUpdateByIdAsync(dto, id);

            return Ok(updatedEntity);
        }

        [HttpPut("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Update an existing category", Description = "Permission: Admin only")]
        [SwaggerResponse(200, "The category was updated successfully", typeof(GetCategoryDto))]
        [SwaggerResponse(201, "The category was created successfully", typeof(GetCategoryDto))]
        [SwaggerResponse(400, "Validation failed", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The was not found", typeof(ProblemDetails))]
        [SwaggerResponse(409, "The category already exists", typeof(ProblemDetails))]
        public async Task<ActionResult<GetCategoryDto>> UpdateAsync(int id, [FromForm] CreateOrUpdateCategoryDto dto)
        {
            var validationResult = new CreateOrUpdateCategoryDtoValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var relativeFilePath = await _fileService.SaveFileAsync(dto.CategoryImage, "category");
            dto.CategoryImagePath = relativeFilePath;

            var entity = await _categoryService.UpsertAsync(dto, id);
            if (entity.Id != id)
            {
                return CreatedAtAction(nameof(GetItem), new { id = entity.Id }, entity);
            }
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a category by id", Description = "Permission: Admin only")]
        [SwaggerResponse(204, "The category was deleted successfully")]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(403, "Forbidden", typeof(ProblemDetails))]
        [SwaggerResponse(404, "The category was not found", typeof(ProblemDetails))]
        public override async Task<ActionResult<Category>> DeleteItem(int id)
        {
            return await base.DeleteItem(id);
        }
    }

}