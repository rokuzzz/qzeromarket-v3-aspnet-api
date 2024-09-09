using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;
using Ecommerce.Services.CategoryService.DTO;
using Ecommerce.Services.Common.Interfaces;

namespace Ecommerce.Services.CategoryService.Interfaces
{
    public interface ICategoryService : IBaseService<Category, CategoryFilterOptions, GetCategoryDto>
    {
        Task<GetCategoryDto> UpsertAsync(CreateOrUpdateCategoryDto dto, int? id = null);
        Task<GetCategoryDto> PartialUpdateByIdAsync(PartialUpdateCategoryDto dto, int id);
    }
}