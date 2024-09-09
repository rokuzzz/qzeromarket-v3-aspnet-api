using FluentValidation;
using Ecommerce.Domain.Models;

namespace Ecommerce.Services.UserService.DTO
{

  public class UpdateUserDto
  {
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
    public string? Avatar { get; set; }
  }

  public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
  {
    public UpdateUserDtoValidator()
    {
      RuleFor(x => x.Email).NotEmpty().EmailAddress();
      RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(100);
      RuleFor(x => x.LastName).NotEmpty().MinimumLength(2).MaximumLength(100);
      RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(20);
    }
  }
}