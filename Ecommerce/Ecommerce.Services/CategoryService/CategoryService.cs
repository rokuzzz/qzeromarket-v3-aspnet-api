using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Services.CategoryService.DTO;
using Ecommerce.Services.CategoryService.Interfaces;
using Ecommerce.Services.Common;
using Ecommerce.Services.Extensions;

namespace Ecommerce.Services.CategoryService
{
    public class CategoryService(ICategoryRepo categoryRepo) : BaseService<Category, CategoryFilterOptions, GetCategoryDto>(categoryRepo), ICategoryService
    {
        private readonly ICategoryRepo _repo = categoryRepo;

        public async Task<GetCategoryDto> PartialUpdateByIdAsync(PartialUpdateCategoryDto dto, int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            var updatedEntity = dto.ToCategory(entity);
            var result = await _repo.PartialUpdateByIdAsync(updatedEntity, id);
            return result.ToGetCategoryDto();
        }

        public async Task<GetCategoryDto> UpsertAsync(CreateOrUpdateCategoryDto dto, int? id = null)
        {
            var result = await _repo.UpsertAsync(dto.ToCategory(id), id);
            return result.ToGetCategoryDto();
        }
    }
}