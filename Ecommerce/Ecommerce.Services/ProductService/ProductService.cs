using Ecommerce.Domain.Filters;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Models;
using Ecommerce.Services.ProductService.DTO;
using Ecommerce.Services.ProductService.Interfaces;
using Ecommerce.Services.Common;
using Ecommerce.Services.Extensions;

namespace Ecommerce.Services.ProductService
{
    public class ProductService(IProductRepo ProductRepo) : BaseService<Product, ProductFilterOptions, GetProductDto>(ProductRepo), IProductService
    {
        private readonly IProductRepo _repo = ProductRepo;

        public async Task<GetProductDto> PartialUpdateByIdAsync(PartialUpdateProductDto dto, int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            var updatedEntity = dto.ToProduct(entity);
            var result = await _repo.PartialUpdateByIdAsync(updatedEntity, id);
            return result.ToGetProductDto();
        }

        public async Task<GetProductDto> UpsertAsync(CreateOrUpdateProductDto dto, int? id = null)
        {
            var result = await _repo.UpsertAsync(dto.ToProduct(id), id);
            return result.ToGetProductDto();
        }
    }
}