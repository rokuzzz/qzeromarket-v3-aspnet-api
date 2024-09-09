using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Models;
using Ecommerce.Services.ProductService.DTO;
using Ecommerce.Services.Common.Interfaces;

namespace Ecommerce.Services.ProductService.Interfaces
{
    public interface IProductService : IBaseService<Product, ProductFilterOptions, GetProductDto>
    {
        Task<GetProductDto> UpsertAsync(CreateOrUpdateProductDto dto, int? id = null);
        Task<GetProductDto> PartialUpdateByIdAsync(PartialUpdateProductDto dto, int id);
    }
}