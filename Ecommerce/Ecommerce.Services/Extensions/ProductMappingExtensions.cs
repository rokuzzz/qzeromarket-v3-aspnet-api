using Ecommerce.Domain.Models;
using Ecommerce.Services.ProductService.DTO;

namespace Ecommerce.Services.Extensions
{
    public static class ProductMappingExtensions
    {
        public static Product ToProduct(this CreateOrUpdateProductDto dto, int? id)
        {
            return new Product
            {
                Id = id ?? 0,
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId,
                ProductImages = dto.ProductImagePath?.Select(x => new ProductImage { Url = x, Alt = "" }).ToList() ?? [],
            };
        }

        public static Product ToProduct(this PartialUpdateProductDto dto, Product entity)
        {
            entity.Title = dto.Title ?? entity.Title;
            entity.Description = dto.Description ?? entity.Description;
            entity.Price = dto.Price ?? entity.Price;
            entity.Stock = dto.Stock ?? entity.Stock;
            entity.CategoryId = dto.CategoryId ?? entity.CategoryId;

            return entity;
        }

        public static GetProductDto ToGetProductDto(this Product entity)
        {
            return new GetProductDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Price = entity.Price,
                Stock = entity.Stock,
                CategoryId = entity.CategoryId,
                ProductImage = entity.ProductImages.Select(x => new GetProducImageDto { Url = x.Url }).ToList(),
            };
        }
    }
}