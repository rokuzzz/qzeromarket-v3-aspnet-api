using FluentValidation;

namespace Ecommerce.Services.OrderService.DTO
{
    public class CreateOrderDto
    {
        public required int UserId { get; set; }
    }

    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}