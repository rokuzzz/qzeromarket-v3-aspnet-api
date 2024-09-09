using FluentValidation;

namespace Ecommerce.Services.CartItemService.DTO
{
    public class PartialUpdateCartItemDto
    {
        public required int Quantity { get; set; }
    }

    public class PartialUpdateCartItemDtoValidator : AbstractValidator<PartialUpdateCartItemDto>
    {
        public PartialUpdateCartItemDtoValidator()
        {
            RuleFor(x => x.Quantity).NotNull().GreaterThan(0);
        }
    }
}