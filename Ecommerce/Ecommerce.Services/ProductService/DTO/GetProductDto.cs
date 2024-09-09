using Ecommerce.Domain.Models;
using Ecommerce.Services.Common.DTO;

namespace Ecommerce.Services.ProductService.DTO
{
    public class GetProducImageDto
    {
        public required string Url { get; set; }
    }
    public class GetProductDto : IReadDto<Product>
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public required int CategoryId { get; set; }
        public List<GetProducImageDto> ProductImage { get; set; } = [];



        public void FromEntity(Product entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Description = entity.Description;
            Price = entity.Price;
            Stock = entity.Stock;
            CategoryId = entity.CategoryId;
            ProductImage = entity.ProductImages?.Select(x => new GetProducImageDto
            {
                Url = x.Url
            }).ToList() ?? [];
        }
    }
}