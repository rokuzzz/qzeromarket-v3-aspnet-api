using FluentValidation;

namespace Ecommerce.Services.CartItemService.DTO
{
    public class CreateCartItemDto
    {
        public required int Quantity { get; set; }
        public required int UserId { get; set; }
        public required int ProductId { get; set; }
    }

    public class CreateCartItemDtoValidator : AbstractValidator<CreateCartItemDto>
    {
        public CreateCartItemDtoValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}