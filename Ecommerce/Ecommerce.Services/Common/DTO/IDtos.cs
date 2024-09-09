using Ecommerce.Domain.Common;

namespace Ecommerce.Services.Common.DTO
{

    public interface IReadDto<T> where T : BaseEntity
    {
        public int Id { get; set; }

        void FromEntity(T entity);
    }
}